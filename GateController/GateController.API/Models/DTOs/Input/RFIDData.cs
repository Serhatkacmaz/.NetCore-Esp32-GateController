using GateController.API.Middlewares.WS;
using System.Text.Json.Serialization;

namespace GateController.API.Models.DTOs.Input
{
    public class RFIDData:BaseMessage
    {
        [JsonPropertyName("cId")]
        public string CardId { get; set; }
    }
}
