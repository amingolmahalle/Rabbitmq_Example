
namespace RabbitMq_Common.RabbitMq
{
    public class ServiceOptionBuilder
    {
        private IServiceOption Option { get; }

        public ServiceOptionBuilder(IServiceOption option)
        {
            Option = option;
        }
        
        public static ServiceOptionBuilder New()
        {
            return new ServiceOptionBuilder(new ServiceOption());
        }
        
        public ServiceOptionBuilder WithTarget(string target)
        {
            Option.Target = target;
            
            return this;
        }
        
        public ServiceOptionBuilder WithBodyType(string bodyType)
        {
            Option.BodyType = bodyType;
            
            return this;
        }
        
        public ServiceOptionBuilder WithCorrelation(string correlationId)
        {
            Option.CorrelationId = correlationId;
            
            return this;
        }
        
        public IServiceOption Build()
        {
            return Option;
        }
    }
}