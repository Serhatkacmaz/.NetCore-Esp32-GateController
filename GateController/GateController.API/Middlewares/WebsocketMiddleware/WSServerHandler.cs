using GateController.API.Middlewares.WS;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace GateController.API.Middlewares.WebsocketMiddleware
{
    public class WSServerHandler
    {
        public readonly Action<string, WebSocket, WebSocketReceiveResult, byte[]> handleMessage;
        private readonly IConfiguration _config;
        private readonly WSServerActions _actions;
        private readonly WSServerConnectionManager _connectionManager;
        private readonly ILogger<WSServerHandler> _logger;
        public WSServerHandler(IConfiguration config, WSServerActions actions, WSServerConnectionManager manager, ILogger<WSServerHandler> logger)
        {
            handleMessage = new Action<string, WebSocket, WebSocketReceiveResult, byte[]>(MessageHandler);
            _connectionManager = manager;
            _config = config;
            _actions = actions;
            _logger = logger;
        }

        public async void MessageHandler(string clientId, WebSocket webSocket, WebSocketReceiveResult result, byte[] buffer)
        {
            switch (result.MessageType)
            {
                case WebSocketMessageType.Text:
                    await TextMessage(clientId, webSocket, result, buffer);
                    break;
                case WebSocketMessageType.Close:
                    await CloseMessage(clientId, webSocket);
                    break;
                default:
                    break;
            }
        }

        private async Task TextMessage(string userID, WebSocket webSocket, WebSocketReceiveResult result, byte[] buffer)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            _logger.LogWarning($"Message Received - {message}");
            try
            {
                var baseMessage = JsonSerializer.Deserialize<BaseMessage>(message);
                if (baseMessage == null)
                {
                    // TODO: Send Error Message And Write Log
                    await SendMessage(webSocket, JsonSerializer.Serialize(new { Type = BaseMessage.MessageType.ErrorMessage, Data = new { ErrorMessage = "Message body is invalid.", ErrorCode = 1000 } }));
                    return;
                }
                var actionResult = _actions.messageActions.TryGetValue(baseMessage.Messagetype, out MessageAction messageActionDeletegate);

                if (!actionResult)
                {
                    // TODO: Send Error Message And Write Log
                    await SendMessage(webSocket, JsonSerializer.Serialize(new { Type = BaseMessage.MessageType.ErrorMessage, Data = new { ErrorMessage = "Received message type is not defined.", ErrorCode = 1000} }));
                    return;
                }
                await messageActionDeletegate!.Invoke(webSocket, userID, baseMessage);
            }
            catch (Exception e)
            {
                _logger.LogError("{clientId} - {err}", userID, e.Message);
                _logger.LogError("Stack Trace: {stc}", e.StackTrace);
            }
        }

        private async Task SendMessage(WebSocket socket, string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(messageBytes, WebSocketMessageType.Text, true, new CancellationTokenSource(1000 * 10).Token);
            }
        }


        private async Task CloseMessage(string clientId, WebSocket webSocket)
        {
            try
            {
                await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

                _connectionManager.RemoveSocket(clientId);
            }
            catch(Exception ex)
            {
                _logger.LogError("{clientId} - Socket close message not delivered", clientId);
            }
            
        }
    }
}
