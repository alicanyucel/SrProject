namespace TELERADIOLOGY.Domain.Entities
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
