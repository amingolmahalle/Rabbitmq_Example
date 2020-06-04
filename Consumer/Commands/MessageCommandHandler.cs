using System;
using System.Text;
using Common.RabbitMQ;
using RabbitMQ.Client;

namespace Consumer.Commands
{
    public class MessageCommandHandler : DefaultBasicConsumer
    {
        private readonly IModel _channel;
        
        private readonly IRabbitMqBus _rabbitMqBus;

        public MessageCommandHandler(IModel channel, IRabbitMqBus serviceBus)
        {
            _channel = channel;
            _rabbitMqBus = serviceBus;
        }

        public override void HandleBasicDeliver(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IBasicProperties properties,
            ReadOnlyMemory<byte> body)
        {
            _rabbitMqBus.Receive("testQueue", this);

            Console.WriteLine($"Consuming Message");
            Console.WriteLine(string.Concat("Message received from the exchange ", exchange));
            Console.WriteLine(string.Concat("Consumer tag: ", consumerTag));
            Console.WriteLine(string.Concat("Delivery tag: ", deliveryTag));
            Console.WriteLine(string.Concat("Routing tag: ", routingKey));
            Console.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(body.ToArray())));

            _channel.BasicAck(deliveryTag, false);
        }
    }
}