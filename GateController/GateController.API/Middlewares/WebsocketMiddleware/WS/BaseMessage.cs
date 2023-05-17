using System.Text.Json;
using System.Text.Json.Serialization;

namespace GateController.API.Middlewares.WS
{
    public class BaseMessage
    {
        [JsonPropertyName("mT")]
        public MessageType Messagetype { get; set; }
        [JsonPropertyName("d")]
        public JsonElement Data { get; set; }

        public T GetData<T>() => Data.Deserialize<T>()!;

        public enum MessageType
        {
            KeepAliveMessage = 1,
            LoginMessage = 5,
            RegisterMessage = 10,
            GateLog = 15,
            NewCardMessage = 16,
            ErrorMessage = 500
        }
    }
}
