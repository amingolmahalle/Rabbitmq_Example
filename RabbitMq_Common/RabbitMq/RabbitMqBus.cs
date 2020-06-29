using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMq_Common.RabbitMq.AssemblyScanner;
using RabbitMq_Common.RabbitMq.Routing;

namespace RabbitMq_Common.RabbitMq
{
    public class RabbitMqBus : IRabbitMqBus
    {
        private readonly IModel _channel;

        private readonly string _endpointId;

        private readonly string _endpointName;

        private readonly QueueDeclareOk _queue;

        private readonly IRouteProvider _routeProvider;

        public RabbitMqBus(string endpointId, string endpointName)
        {
            _endpointId = endpointId;
            _endpointName = endpointName;
            _routeProvider = RouteProvider.GetInstance();
            var connection = new RabbitMqConnection().TryConnection();
            _channel = connection.CreateModel();

            _queue = _channel.QueueDeclare(_endpointName, true, false, false, null);

            StartReceiver();
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
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: props,
                body: message);
        }

        private void Subscribe()
        {
            var events = Types.GetHandlers()
                .SelectMany(it => it.GetInterfaces()
                    .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandleCommand<>)))
                .Select(it => it.GenericTypeArguments[0])
                .ToList();

            foreach (var @event in events)
            {
                _channel.ExchangeDeclare(exchange: @event.FullName, type: ExchangeType.Direct);
                _channel.QueueBind(queue: _queue.QueueName, exchange: @event.FullName, routingKey: _queue.QueueName);
            }
        }

        private void StartReceiver()
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += Message_Received;

            _channel.BasicConsume(queue: _queue.QueueName, autoAck: true, consumer: consumer);
        }

        private void Message_Received(object sender, BasicDeliverEventArgs e)
        {
        }

        private static byte[] PopulateMessage(object message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }
    }
}