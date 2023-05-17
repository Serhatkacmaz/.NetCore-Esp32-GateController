using System.Text.Json.Serialization;
using static GateController.API.Middlewares.WS.BaseMessage;

namespace GateController.API.Models.DTOs.Output
{
    public class LoginResponse
    {
        [JsonPropertyName("mT")]
        public MessageType Messagetype { get; set; } = MessageType.LoginMessage;
        public int MyProperty { get; set; }
    }
}
