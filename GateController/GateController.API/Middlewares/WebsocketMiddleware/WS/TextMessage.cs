namespace GateController.API.Middlewares.WS
{
    public class TextMessage
    {
        public string Message { get; set; }
        public long GroupId { get; set; }
        public long TargetUser { get; set; }
        public string MessageSign { get; set; }
    }
}
