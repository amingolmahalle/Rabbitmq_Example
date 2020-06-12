using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Common.AssemblyScanner;
using Common.Attributes;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.RabbitMq
{
    public class RabbitMqBus : IRabbitMqBus
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;

        private IModel _consumerChannel;

        private string _queueName;

        private string _routingKey;

        private string _exchangeName;

        private readonly string _endpointId;

        private readonly string _endpointName;

        //private MethodInfo _consumeMethod;

        //private object _consumer;

        public RabbitMqBus(string endpointId, string endpointName)
        {
            _rabbitMqConnection = new RabbitMqConnection();
            _endpointId = endpointId;
            _endpointName = endpointName;

          //  StartReceiver();
            Subscribe();
        }

        public void Send(object message)
        {
            SendInternal(message, _routingKey, _exchangeName);
        }

        private void SendInternal(
            object message,
            string routingKey,
            string exchange = "")
        {
            SendBytes(PopulateMessage(message), message.GetType(), routingKey, exchange);
        }

        private void SendBytes(
            byte[] message,
            Type messageType,
            string routingKey,
            string exchange = "")
        {
            _consumerChannel = CreateChannel(messageType);
            var props = _consumerChannel.CreateBasicProperties();

            // props.Headers=null 
            props.MessageId = Guid.NewGuid().ToString();
            props.ContentType = "application/json";
            props.ContentEncoding = "utf8";
            props.AppId = _endpointId;
            props.CorrelationId = props.CorrelationId == null || string.IsNullOrEmpty(props.CorrelationId)
                ? Guid.NewGuid().ToString()
                : props.CorrelationId;

            _consumerChannel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: props,
                body: message);
        }

        private void Subscribe()
        {
            var typesToRegister = Types.GetHandlers();

            foreach (var item in typesToRegister)
            {
                // var message = JsonConvert.DeserializeObject(messageb, messageType);
                // item.GetMethod("Handle")?.Invoke(Activator.CreateInstance(item), 
                //     new[] { message });
                
                
                //var consumer = item.GetConstructor(Type.EmptyTypes);
                //
                // if (consumer != null)
                //  _consumer = consumer.Invoke(new object[] { });

                //_consumeMethod = item.GetMethod("Handle");

               // StartReceiver(item);
            }
        }

        private void StartReceiver(Type @event)
        {
            using (_consumerChannel = CreateChannel(@event))
            {
                var consumer = new EventingBasicConsumer(_consumerChannel);

                consumer.Received += Message_Received;

                _consumerChannel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            }
        }

        private void Message_Received(object sender, BasicDeliverEventArgs e)
        {
            // if (_consumeMethod != null)
            // {
            //     _consumeMethod.Invoke(_consumer, new[]
            //     {
            //         JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Body.ToArray()),
            //             _consumeMethod.GetParameters()
            //                 .FirstOrDefault()?.ParameterType)
            //     });
            //}
        }

        private IModel CreateChannel(Type @event)
        {
            Initialize(@event);

            _rabbitMqConnection.TryConnection();

            var channel = _rabbitMqConnection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Direct);
            channel.QueueDeclare(_queueName, true, false, false, null);
            channel.QueueBind(_queueName, _exchangeName, _routingKey, null);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateChannel(@event);
            };

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
    }
}