using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Common.RabbitMq.Routing
{
    public class RouteProvider : IRouteProvider
    {
        private readonly List<MessageRouteTableValue> _routes = new List<MessageRouteTableValue>();

        private static readonly object Padlock = new object();

        private static RouteProvider _instance;

        private RouteProvider()
        {
        }

        public static RouteProvider GetInstance()
        {
            if (_instance == null)
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new RouteProvider();
                    }
                }
            }

            return _instance;
            }

        public string GetConsumer(object message, IServiceOption options = null)
        {
            return GetNearestMatch(message.GetType().AssemblyQualifiedName, options);
        }

        private string GetNearestMatch(string messageType, IServiceOption options = null)
        {
            if (options != null && !string.IsNullOrEmpty(options.Target))
                return options.Target;

            var consumer = _routes.Where(it => messageType.StartsWith(it.Namespace))
                .Select(it => new
                {
                    it.Consumer,
                    it.Namespace.Length
                })
                .OrderBy(it => it.Length)
                .Select(it => it.Consumer)
                .LastOrDefault();

            if (consumer == null)
                throw new ArgumentOutOfRangeException($"for message type of {messageType} did not config any route");

            return consumer;
        }

        public void AddRouteFromConfigFile(string fileName = "appsettings.json")
        {
            try
            {
                var cfg = new ConfigurationBuilder()
                    .AddJsonFile(fileName)
                    .Build();

                var cfgVal = cfg.GetSection("MessageRouteMappings");

                if (cfgVal == null)
                    return;

                foreach (var mapping in cfgVal.GetChildren())
                {
                    AddCommandRoute(mapping.Key, mapping.Value);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException($"MessageRouteMappings not found : {ex.InnerException}");
            }
        }

        public void AddCommandRoute(string @namespace, string consumer)
        {
            _routes.Add(new MessageRouteTableValue
            {
                Namespace = @namespace,
                Type = MessageType.Command,
                Consumer = consumer
            });
        }
    }
}