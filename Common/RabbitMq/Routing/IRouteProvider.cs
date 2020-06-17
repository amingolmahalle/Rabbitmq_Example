namespace Common.RabbitMq.Routing
{
    public interface IRouteProvider
    {
        string GetConsumer(object message, IServiceOption options = null);
        void AddCommandRoute(string @namespace, string consumer);
        void AddRouteFromConfigFile(string fileName = "appsettings.json");
    }
}