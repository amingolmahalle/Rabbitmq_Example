using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.Attributes;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.RabbitMq
{
    public class RabbitMqBus : IRabbitMqBus
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;

        private string _queueName;

        private string _routingKey;

        private string _exchangeName;

        private readonly string _endpointId;

        private readonly string _endpointName;

        private MethodInfo _consumeMethod;

        private object _consumer;

        public RabbitMqBus(string endpointId, string endpointName)
        {
            _rabbitMqConnection = new RabbitMqConnection();
            _endpointId = endpointId;
            _endpointName = endpointName;

            Subscribe();
        }

        public void Send(object message)
        {
            Initialize(message.GetType());

            SendInternal(message, _routingKey, _exchangeName);
        }

        private void SendInternal(
            object message,
            string routingKey,
            string exchange = "")
        {
            SendBytes(PopulateMessage(message), routingKey, exchange);
        }

        private void SendBytes(
            byte[] message,
            string routingKey,
            string exchange = "")
        {
            var channel = CreateChannel();
            var props = channel.CreateBasicProperties();

            // props.Headers=null 
            props.MessageId = Guid.NewGuid().ToString();
            props.ContentType = "application/json";
            props.ContentEncoding = "utf8";
            props.AppId = _endpointId;
            props.CorrelationId = props.CorrelationId == null || string.IsNullOrEmpty(props.CorrelationId)
                ? Guid.NewGuid().ToString()
                : props.CorrelationId;

            channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: props,
                body: message);
        }
        
        private void Subscribe()
        {
            var typesToRegister = AllTypes.Where(it => it.IsClass).ToList();
            typesToRegister.ForEach(it =>
            {
                if (it.GetInterfaces().Any(y => y == typeof(IHandleCommand)))
                {
                    var consumer = it.GetConstructor(Type.EmptyTypes);

                    if (consumer != null)
                        _consumer = consumer.Invoke(new object[] { });

                    _consumeMethod = it.GetMethod("Handle");

                    Receive(it);
                }
            });
        }
        
        public void Receive(Type @event)
        {
            Initialize(@event);

            var channel = CreateChannel();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;
            channel.BasicConsume(_queueName,
                true,
                consumer);
        }

        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
        }

        private IModel CreateChannel()
        {
            _rabbitMqConnection.TryConnection();

            var channel = _rabbitMqConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);
            channel.QueueDeclare(_routingKey, true, false, false, null);
            channel.QueueBind(_routingKey, _exchangeName, _routingKey, null);

            return channel;
        }

        private void Initialize(Type @event)
        {
            foreach (Attribute attribute in @event.GetCustomAttributes(true))
            {
                if (!(attribute is QueueAttribute queue))
                    continue;

                _routingKey = queue.RoutingKey;
                _exchangeName = string.IsNullOrEmpty(queue.ExchangeName) ? string.Empty : queue.ExchangeName;
                _queueName = string.IsNullOrEmpty(queue.QueueName) ? _endpointName : queue.QueueName;
            }
        }

        private static byte[] PopulateMessage(object message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }
        
        private static IEnumerable<Type> AllTypes
        {
            get { return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()); }
        }
    }
}