using GateController.API.Middlewares.WebsocketMiddleware;
using GateController.API.Middlewares.WS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace GateController.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteGateController : ControllerBase
    {
        private readonly WSServerConnectionManager _conectionManager;
        public RemoteGateController(WSServerConnectionManager connectionManager)
        {
            _conectionManager = connectionManager;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> SendRegisterSignal()
        {
            var deviceConnection = _conectionManager.WSConnections.FirstOrDefault(x => x.Value.ClientType == Middlewares.WS.ClientType.Device);
            if (deviceConnection.Key == null)
                return BadRequest();

            await SendMessageAsync(deviceConnection.Value.Socket, JsonSerializer.Serialize(new { mT = BaseMessage.MessageType.RegisterMessage, d = string.Empty }));
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult RemoveAllConnection()
        {
            _conectionManager.WSConnections.Clear();
            return Ok();
        }

        [HttpPost("[action]")]
        public IActionResult GetAllConnection() => Ok(_conectionManager.WSConnections);
        

        [NonAction]
        private async Task SendMessageAsync(WebSocket socket, string message)
        {
            await socket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, new CancellationTokenSource(1000 * 10).Token);
        }
    }
}
