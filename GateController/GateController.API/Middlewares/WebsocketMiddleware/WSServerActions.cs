using GateController.API.Middlewares.WS;
using GateController.API.Models.DTOs.Input;
using GateController.Repository;
using GateController.Repository.Entities;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;


namespace GateController.API.Middlewares.WebsocketMiddleware
{
    public delegate Task MessageAction(WebSocket webSocket, string userID, BaseMessage baseMessage);
    public class WSServerActions
    {
        private readonly WSServerConnectionManager _connectionManager;
        public readonly ConcurrentDictionary<BaseMessage.MessageType, MessageAction> messageActions;
        private readonly SystemCardsRepository _cardsRepository;
        private readonly GateLogRepository _gateLogRepository;
        private readonly ILogger<WSServerActions> _logger;
        public WSServerActions(WSServerConnectionManager connectionManager, ILogger<WSServerActions> logger, SystemCardsRepository cardRepo, GateLogRepository gateLogRepo)
        {
            _connectionManager = connectionManager;
            _cardsRepository = cardRepo;
            _gateLogRepository = gateLogRepo;
            messageActions = new ConcurrentDictionary<BaseMessage.MessageType, MessageAction>();
            messageActions.TryAdd(BaseMessage.MessageType.KeepAliveMessage, new MessageAction(KeepAliveMessageReceived));
            messageActions.TryAdd(BaseMessage.MessageType.RegisterMessage, new MessageAction(RegisterMessageAction));
            messageActions.TryAdd(BaseMessage.MessageType.LoginMessage, new MessageAction(LoginMessageAction));
            _logger = logger;
        }

        public Task KeepAliveMessageReceived(WebSocket webSocket, string clientId, BaseMessage message)
        {
            var connection = _connectionManager.WSConnections.TryGetValue(clientId, out SocketNode userSocket);
            if (connection)
            {
                userSocket!.LastSignalTime = DateTime.Now;
            }
            return Task.CompletedTask;
        }

        public async Task RegisterMessageAction(WebSocket webSocket, string clientId, BaseMessage message)
        {
            var registerMessage = message.GetData<RFIDData>();

            var conenctions = _connectionManager.WSConnections.Where(x => x.Value.ClientType == ClientType.PanelUser);
            foreach (var connection in conenctions)
            {
                await SendMessage(connection.Value.Socket, JsonSerializer.Serialize(new { Date = DateTime.Now, CardHex = registerMessage.CardId, MessageType = BaseMessage.MessageType.NewCardMessage }));
            }
        }

        public async Task LoginMessageAction(WebSocket websocket, string clientId, BaseMessage message)
        {
            var registerMessage = message.GetData<RFIDData>();
            var card = _cardsRepository.GetCard(registerMessage.CardId);
            if (card == null)
            {
                await SendMessage(websocket, JsonSerializer.Serialize(new
                {
                    mT = BaseMessage.MessageType.LoginMessage,
                    d = new { rs = false }
                }));
            }
            else
            {
                await _gateLogRepository.InsertLogAsync(card.Id, card.OfficeMemberId);
                await SendMessage(websocket, JsonSerializer.Serialize(new
                {
                    mT = BaseMessage.MessageType.LoginMessage,
                    d = new { rs = true, mn = card.OfficeMember.Name }
                }));
            }
        }

        private async Task SendMessage(WebSocket socket, string message)
        {
            await socket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, new CancellationTokenSource(1000 * 10).Token);
        }
    }
}
