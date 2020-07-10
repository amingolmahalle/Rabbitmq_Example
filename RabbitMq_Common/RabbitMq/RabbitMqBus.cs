using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMq_Common.RabbitMq.AssemblyScanner;
using RabbitMq_Common.RabbitMq.Routing;

namespace RabbitMq_Common.RabbitMq
{
    public class RabbitMqBus : IRabbitMqBus
    {
        private IModel _channel;

        private readonly string _endpointId;

        private readonly string _endpointName;

        private QueueDeclareOk _queue;

        private readonly IRouteProvider _routeProvider;

        public RabbitMqBus(string endpointId, string endpointName)
        {
            _endpointId = endpointId;
            _endpointName = endpointName;
            _routeProvider = RouteProvider.GetInstance();
            CreateChannel();
            //  StartReceiver();
            Subscribe();
        }

        public void Send(object message)
        {
            SendInternal(message);
        }

        private void SendInternal(object message)
        {
            SendBytes(PopulateMessage(message), _routeProvider.GetConsumer(message), string.Empty);
        }

        private void SendBytes(byte[] message, string routingKey, string exchange)
        {
            var props = _channel.CreateBasicProperties();

            // props.Headers=null 
            props.MessageId = Guid.NewGuid().ToString();
            props.ContentType = "application/json";
            props.ContentEncoding = "utf8";
            props.AppId = _endpointId;
            props.CorrelationId = props.CorrelationId == null || string.IsNullOrEmpty(props.CorrelationId)
                ? Guid.NewGuid().ToString()
                : props.CorrelationId;

            _channel.BasicPublish(
                exchange: "hello",
                routingKey: routingKey,
                basicProperties: props,
                body: message);
        }

        public void Subscribe()
        {
            var handlers = Types.GetHandlers();

            foreach (var handler in handlers)
            {
                handler.GetMethod("Handle")
                    ?.Invoke(Activator.CreateInstance(handler),
                        null);
                StartReceiver();
            }
        }

        private void CreateChannel()
        {
            var connection = new RabbitMqConnection().TryConnection();

            _channel = connection.CreateModel();
            _queue = _channel.QueueDeclare(_endpointName, true, false, false, null);
            _channel.ExchangeDeclare(exchange: "hello", type: ExchangeType.Direct);
            _channel.QueueBind(queue: _queue.QueueName, exchange: "hello", routingKey: _queue.QueueName, null);
        }

        private void StartReceiver()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += Message_Received;
            consumer.Received += (model, ea) =>
            {
                _channel.BasicConsume(queue: _queue.QueueName, autoAck: true, consumer: consumer);
            };
        }

        private void Message_Received(object sender, BasicDeliverEventArgs e)
        {
            Task.Factory.StartNew(async () =>
            {
                Console.WriteLine($"new message just received. {JsonConvert.SerializeObject(e)}");
            });
        }

        private static byte[] PopulateMessage(object message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}