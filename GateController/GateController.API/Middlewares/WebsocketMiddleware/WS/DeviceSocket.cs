using GateController.API.Middlewares.WS;
using System.Net.WebSockets;

namespace GateController.API.Middlewares.WebsocketMiddleware.WS
{
    public class DeviceSocket:SocketNode
    {
        public DeviceSocket(WebSocket socket, DateTime lastSignalTime):base(socket, lastSignalTime, ClientType.Device)
        {

        }
    }
}
