using System;
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

        public RabbitMqBus(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
          //  Subscribe();
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
            _rabbitMqConnection.TryConnection();

            var channel = CreateChannel();
            var props = channel.CreateBasicProperties();

            // props.Headers=null 
            props.MessageId = Guid.NewGuid().ToString();
            props.ContentType = "application/json";
            props.ContentEncoding = "utf8";
            //props.AppId = EndpointId; // ba51ac1b-d974-4722-a639-a967514478d8
            props.CorrelationId = props.CorrelationId == null || string.IsNullOrEmpty(props.CorrelationId)
                ? Guid.NewGuid().ToString()
                : props.CorrelationId;

            channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: props,
                body: message);
        }

        private byte[] PopulateMessage(object message)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        }

        public void Receive(Type @event)
        {
            _rabbitMqConnection.TryConnection();

            Initialize(@event);

            var channel = CreateChannel();

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(_queueName,
                true,
                consumer);
        }

        private void Subscribe(Assembly getExecutingAssembly)
        {
            // _getExecutingAssembly = getExecutingAssembly;
            //
            // var items = getExecutingAssembly.GetExportedTypes().Where(x => x.IsClass).ToList();
            // items.ForEach(x =>
            // {
            //     if (x.GetInterfaces().Any(y => y == typeof(IHandleCommand<>)))
            //     {
            //         var consumer = x.GetConstructor(Type.EmptyTypes);
            //
            //         if (consumer != null)
            //             _consumer = consumer.Invoke(new object[] { });
            //
            //         _consumeMethod = x.GetMethod("Handle");
            //
            //         Receive(x);
            //     }
            // });
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

                _routingKey = queue.QueueName ?? @event.Name;
                _exchangeName = queue.ExchangeName;
                _queueName = queue.QueueName;
            }
        }
    }
}