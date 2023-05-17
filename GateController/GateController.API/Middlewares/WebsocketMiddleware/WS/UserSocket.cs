using System.Net.WebSockets;

namespace GateController.API.Middlewares.WS
{
    public class UserSocket:SocketNode
    {
        public UserSocket(WebSocket socket, DateTime lastSignalTime):base(socket, lastSignalTime, ClientType.PanelUser)
        {

        }
    }
}
