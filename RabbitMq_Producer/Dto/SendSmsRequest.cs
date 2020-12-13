namespace RabbitMq_Producer.Dto
{
    public class SendSmsRequest
    {
        public string Message { get; set; }
        
        public string Mobile { get; set; }   
    }
}