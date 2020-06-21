namespace RabbitMq_Common.RabbitMq.Routing
{
    public interface IRouteProvider
    {
        string GetConsumer(object message, IServiceOption options = null);
        void AddRouteFromConfigFile(string fileName = "appsettings.json");
    }
}