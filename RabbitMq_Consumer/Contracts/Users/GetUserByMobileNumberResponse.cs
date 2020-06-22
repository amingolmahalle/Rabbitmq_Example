namespace RabbitMq_Consumer.Contracts.Users
{
    public class GetUserByMobileNumberResponse
    {
        public int Id { get; set; }
        
        public string Fullname { get; set; }
        
        public string MobileNumber { get; set; }
        
        public string Email { get; set; }
        
        public bool IsActive { get; set; }
        
        public int? Age { get; set; }
    }
}