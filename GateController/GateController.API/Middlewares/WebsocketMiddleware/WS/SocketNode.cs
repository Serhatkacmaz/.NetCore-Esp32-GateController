using System.Net.WebSockets;

namespace GateController.API.Middlewares.WS
{
    public abstract class SocketNode
    {
        public DateTime LastSignalTime { get; set; }
        public WebSocket Socket { get; set; }
        public ClientType ClientType { get; set; }
        public SocketNode(WebSocket webSocket, DateTime lastSignalTime, ClientType clientType)
        {
            Socket = webSocket;
            LastSignalTime = lastSignalTime;
            ClientType = clientType;
        }
    }

    public enum ClientType
    {
        Device,
        PanelUser
    }
}
