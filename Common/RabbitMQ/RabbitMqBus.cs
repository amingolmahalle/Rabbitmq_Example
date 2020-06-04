using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Common.RabbitMQ
{
    public class RabbitMqBus : IRabbitMqBus
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;

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
            var channel = _rabbitMqConnection.GetConnection();
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
            var channel = _rabbitMqConnection.GetConnection();

            channel.BasicConsume(queueName,
                false,
                consumerTag,
                false,
                false,
                arguments,
                consumer);
        }
    }
}