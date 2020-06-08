using System;
using System.Collections.Generic;
using System.Text;
using Common.Attributes;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Common.RabbitMq
{
    public class RabbitMqBus : IRabbitMqBus
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        
        private string _queueName;
        
        private string _exchangeName;

        public RabbitMqBus(IRabbitMqConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public void Send(
            object message,
            string routingKey,
            string exchange)
        {
            SendInternal(message, routingKey, exchange);
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
            
            var channel = _rabbitMqConnection.CreateModel();
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

        public void Receive(
            string queueName,
            IBasicConsumer consumer,
            string consumerTag = "",
            IDictionary<string, object> arguments = null)
        {
            _rabbitMqConnection.TryConnection();

            var channel = _rabbitMqConnection.CreateModel();

            channel.BasicConsume(queueName,
                false,
                consumerTag,
                false,
                false,
                arguments,
                consumer);
        }

        private IModel CreateChannel(Type @event)
        {
            Initialize(@event);

            _rabbitMqConnection.TryConnection();

            var channel = _rabbitMqConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);
            channel.QueueDeclare(_queueName, true, false, false, null);
            channel.QueueBind(_queueName, _exchangeName, _queueName, null);

            return channel;
        }

        private void Initialize(Type @event)
        {
            foreach (Attribute attribute in @event.GetCustomAttributes(true))
            {
                if (!(attribute is QueueAttribute queue))
                    continue;

                _queueName = queue.QueueName ?? @event.Name;
                _exchangeName = queue.ExchangeName;
            }
        }
    }
}