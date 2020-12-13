namespace RabbitMq_Producer.Dto
{
    public class SendEmailRequest
    {
        public string Email { get; set; }
        
        public string Subject { get; set; }
        
        public string Message { get; set; }
    }
}