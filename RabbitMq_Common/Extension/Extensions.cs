using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMq_Common.RabbitMq;
using RabbitMq_Common.RabbitMq.Event;

namespace RabbitMq_Common.Extension
{
    public static class Extensions
    {
        public static void TryDispose(this IDisposable disposable, ILogger logger = null)
        {
            try
            {
                disposable.Dispose();
            }
            catch (System.Exception ex)
            {
                logger?.LogWarning("could not dispose: ", ex.Message);
                logger?.LogError(ex, ex.Message);
            }
        }

        public static T GetContext<T>(this IEvent ev, string key)
        {
            if (ev.Context.TryGetValue(key, out object v))
            {
                return (T) v;
            }

            return default;
        }
        
        public static byte[] PopulateMessage(this object message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }

        public static string ObjectSerializer(this IProducerCommand command)
        {
            return JsonConvert.SerializeObject(command, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public static T ObjectDeserializer<T>(this string message)
        {
            return JsonConvert.DeserializeObject<T>(message);
        }
    }
}