using Microsoft.AspNetCore.DataProtection;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Primitives;
using GateController.API.Middlewares.WS;
using GateController.Repository;
using GateController.API.Middlewares.WebsocketMiddleware.WS;

namespace GateController.API.Middlewares.WebsocketMiddleware
{
    public class WSServerMiddleware
    {
        private readonly RequestDelegate _request;
        private readonly WSServerConnectionManager _connectionManager;
        private readonly WSServerHandler _handler;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WSServerMiddleware> _logger;

        public WSServerMiddleware(RequestDelegate request, WSServerConnectionManager connectionManager, WSServerHandler handler, IConfiguration configuration, ILogger<WSServerMiddleware> logger)
        {
            _request = request;
            _connectionManager = connectionManager;
            _handler = handler;
            _configuration = configuration;

            new Thread(DestroyAbortedConnection).Start();
            _logger = logger;
            _logger.LogWarning("Ws Server Started");

        }

        private async Task Receive(WebSocket socket, string clientId)
        {
            var buffer = new byte[1024 * 4];

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);
                    _handler.handleMessage(clientId, socket, result, buffer);
                    // handleMessage(result, buffer);
                }
            }
            catch (Exception)
            {
                var connectionDefined = _connectionManager.WSConnections.TryGetValue(clientId, out SocketNode socketNode);

                _connectionManager.RemoveSocket(clientId);
            }

        }

        public async Task InvokeAsync(HttpContext context, SystemDevicesRepository devicesRepository)
        {
            if (context.WebSockets.IsWebSocketRequest && context.Request.Path.ToString().StartsWith("/ws", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Socket Request - {context.Request.QueryString}");
                var clientTypeDefined = context.Request.Query.TryGetValue("ct", out StringValues ctype);
                if (!clientTypeDefined)
                {
                    // TODO: Send Information
                    context.Response.StatusCode = 400;
                    return;
                }

                if(ctype.ToString() == "1")
                {
                    var dIdDefined = context.Request.Query.TryGetValue("dId", out StringValues deviceId);
                    if (!dIdDefined)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                    var device = devicesRepository.GetDevice(deviceId.ToString());

                    if (device == null)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }

                    var websocket = await context.WebSockets.AcceptWebSocketAsync();
                    await BuildConnection(new DeviceSocket(websocket, DateTime.Now), device.DeviceId);
                }
                else if (ctype.ToString() == "2")
                {
                    var cId = context.Request.Query.TryGetValue("cId", out StringValues clientId);
                    if (!cId)
                    {
                        context.Response.StatusCode = 401;
                        return;
                    }
                    var websocket = await context.WebSockets.AcceptWebSocketAsync();
                    await BuildConnection(new UserSocket(websocket, DateTime.Now), clientId.ToString());
                }
                context.Request.Query.TryGetValue("deviceId", out StringValues value);

                return;
            }
            else
                await _request(context);
        }

        private async Task BuildConnection(SocketNode socket, string clientId)
        {
            var connectionDefined = _connectionManager.WSConnections.TryGetValue(clientId, out var wsConnection);
            if (connectionDefined)
            {
                await CloseMessage(clientId, socket.Socket);
            }
            _connectionManager.AddSocket(socket, clientId.ToString());
            await Receive(socket.Socket, clientId.ToString());
        }

        private async Task CloseMessage(string clientId, WebSocket webSocket)
        {
            try
            {
                await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

            }
            catch (Exception ex)
            {
                _logger.LogError("{clientId} - Socket close message not delivered", clientId);
            }
            _connectionManager.RemoveSocket(clientId);
        }

        private async Task SendMessageAsync(WebSocket socket, string message)
        {
            await socket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, new CancellationTokenSource(1000 * 10).Token);
        }

        private async Task SendMessageAsync(string message, string targetUser)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var userIsOnline = _connectionManager.WSConnections.TryGetValue(targetUser, out SocketNode userSocket);

            if (userIsOnline)
            {
                try
                {
                    await userSocket!.Socket.SendAsync(messageBytes, WebSocketMessageType.Text, true, new CancellationTokenSource(1000 * 10).Token);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // TODO: Log
                }
            }
        }


        private async void DestroyAbortedConnection()
        {
            while (true)
            {
                //foreach (var connection in _connectionManager.WSConnections)
                //{
                //    var userSocket = connection.Value;

                //    if (userSocket.Socket.State == WebSocketState.Aborted || userSocket.Socket.State == WebSocketState.Closed)
                //    {
                //        if (userSocket.IsBusClient)
                //        {
                //            var busSocket = userSocket as BusSocket;
                //            await SendBusClientCloseMessage(busSocket.BusName);
                //        }
                //        _connectionManager.RemoveSocket(connection.Key);

                //    }
                //    else
                //    {
                //        if ((DateTime.Now - userSocket.LastSignalTime).TotalSeconds > 20)
                //        {
                //            try
                //            {
                //                await userSocket.Socket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                //            }
                //            catch (Exception) { }
                //            if (userSocket.IsBusClient)
                //            {
                //                var busSocket = userSocket as BusSocket;
                //                await SendBusClientCloseMessage(busSocket.BusName);
                //            }
                //            _connectionManager.RemoveSocket(connection.Key);

                //        }
                //    }
                //}
                Thread.Sleep(20000);
            }
        }

    }
}
