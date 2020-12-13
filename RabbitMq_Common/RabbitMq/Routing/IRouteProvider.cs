namespace RabbitMq_Common.RabbitMq.Routing
{
    public interface IRouteProvider
    {
        string GetConsumer(object message);
        
        void AddRouteFromConfigFile(string fileName = "appsettings.json");
    }
}