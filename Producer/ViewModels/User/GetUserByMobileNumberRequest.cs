using Common.Attributes;

namespace Producer.ViewModels.User
{
    [Queue(queueName: "Consumer.Host", exchangeName: "testExchange", routingKey: "directExchange_key")]
    public class GetUserByMobileNumberRequest
    {
        public string MobileNumber { get; set; }
    }
}