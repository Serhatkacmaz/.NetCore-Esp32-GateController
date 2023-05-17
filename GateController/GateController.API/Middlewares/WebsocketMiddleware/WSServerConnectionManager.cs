using GateController.API.Middlewares.WS;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace GateController.API.Middlewares.WebsocketMiddleware
{
    public class WSServerConnectionManager
    {
        private ConcurrentDictionary<string, SocketNode> _connections = new ConcurrentDictionary<string, SocketNode>();
      

        public bool AddSocket(SocketNode userSocket, string clientId) => _connections.TryAdd(clientId, userSocket);
        
        public ConcurrentDictionary<string, SocketNode> WSConnections { get => _connections; }

        public bool RemoveSocket(string key)
        {
            return _connections.TryRemove(key, out _);
        }
    }
}
