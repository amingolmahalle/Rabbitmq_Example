using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMq_Common.Exception;
using RabbitMq_Common.Extension;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMq_Common.RabbitMq.Event;
using RabbitMq_Common.RabbitMq.Event.Enums;

namespace RabbitMq_Common.RabbitMq
{
    public class RabbitConnector : IRabbitConnector, IEventAckNack
    {
        private readonly ILogger<RabbitConnector> _logger;

        private readonly string _userName = SystemConstants.Username;

        private readonly string _password = SystemConstants.Password;

        private readonly string _hostName = SystemConstants.HostName;

        private readonly int _port = SystemConstants.Port;

        private readonly int _retryConnectionInitSleepSeconds = SystemConstants.RetryConnectionInitSleepSeconds;

        private readonly bool _autoTopologyRecoveryEnabled = true;

        private readonly bool _autoConnectionRecoveryEnabled = true;

        private static IConnection _connection;

        private static IModel _readChannel;

        private static IModel _writeChannel;

        private bool _initialized;

        private readonly object _initializationLock = new object();

        private readonly IJobDispatcher _jobDispatcher;

        private readonly IEventFactory _eventFactory;

        public RabbitConnector(
            ILogger<RabbitConnector> logger,
            IJobDispatcher jobDispatcher,
            IEventFactory eventFactory)
        {
            _logger = logger;
            _jobDispatcher = jobDispatcher;
            _eventFactory = eventFactory;
        }

        public Task InitializeAsync(string serviceName = "")
        {
            lock (_initializationLock)
            {
                if (_initialized)
                {
                    _logger?.LogTrace("ignoring initialization, already initialized");

                    return Task.FromResult(false);
                }

                ConnectionFactory connectionFactory = new ConnectionFactory()
                {
                    HostName = _hostName,
                    Port = _port,
                    UserName = _userName,
                    Password = _password,
                    AutomaticRecoveryEnabled = _autoConnectionRecoveryEnabled,
                    TopologyRecoveryEnabled = _autoTopologyRecoveryEnabled,
                    ClientProvidedName = serviceName
                };

                while (_connection?.IsOpen != true)
                {
                    try
                    {
                        _logger?.LogInformation("gotta init connection...");

                        if (null == _connection)
                        {
                            _connection = connectionFactory.CreateConnection();
                        }

                        _logger?.LogInformation("connection success to rabbit");
                    }
                    catch (System.Exception ex)
                    {
                        _logger?.LogInformation($"error in connecting to rabbit: {ex.Message}");
                        _logger?.LogError(ex, ex.Message);

                        _logger?.LogInformation($"retrying in {_retryConnectionInitSleepSeconds} seconds");

                        Task.Delay(_retryConnectionInitSleepSeconds * 1000);

                        continue;
                    }

                    _logger?.LogInformation("read/write channel initialized");
                }

                _initialized = true;

                return Task.FromResult(_initialized);
            }
        }

        public Task ConsumeAsync(string queueName)
        {
            if (!_initialized)
            {
                _logger?.LogTrace("connector is not yet initialized, returning");

                return Task.FromResult(false);
            }

            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new InvalidOperationException($"invalid queueName: {queueName}");
            }

            EventingBasicConsumer eventingConsumer = new EventingBasicConsumer(_readChannel);

            _readChannel = _connection.CreateModel();

            _readChannel.QueueDeclare(queueName, true, false, false, null);

            var names = Enum.GetNames(typeof(EventType));
            Type enumType = typeof(EventType);
            bool isConsuming = false;

            foreach (var exchange in names)
            {
                MemberInfo[] memberInfos = enumType.GetMember(exchange);
                MemberInfo valueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);

                if (valueMemberInfo != null)
                    isConsuming = true;

                if (!isConsuming)
                    continue;

                //[SendSms]
                //[SendEmail]
                _readChannel.ExchangeDeclare(exchange, ExchangeType.Direct, true, false, null);
                _readChannel.QueueBind(queueName, exchange, queueName, null);
                //TODO (optional): abstract queueBind class for consumers(prefetch)
            }

            eventingConsumer.Received += MessageReceived;

            _readChannel.BasicConsume(queueName, false, "consumer", true, true, null, eventingConsumer);

            return Task.FromResult(true);
        }

        private void MessageReceived(object sender, BasicDeliverEventArgs e)
        {
            string body = null;

            if (e.Body.Length > 0)
            {
                body = Encoding.UTF8.GetString(e.Body.ToArray());
            }

            try
            {
                var exchange = e.Exchange;

                IEvent ev = _eventFactory.CreateEvent(exchange);

                ev.Payload = body;
                ev.RoutingKey = e.RoutingKey;
                ev.Context.Add("deliveryTag", e.DeliveryTag);
                ev.Context.Add("exchange", exchange);
                ev.EndpointId = e.BasicProperties.AppId;

                _jobDispatcher
                    .DispatchAsync(ev, exchange, this)
                    .ConfigureAwait(true)
                    .GetAwaiter()
                    .GetResult();
            }
            catch (EventNotSupportedException ex)
            {
                _logger?.LogError(ex, ex.Message);

                _readChannel.BasicNack(e.DeliveryTag, false, false);
            }
            catch (CouldNotFindAnyHandlerException ex)
            {
                _logger?.LogError(ex, ex.Message);

                _readChannel.BasicNack(e.DeliveryTag, false, false);
            }
            catch (System.Exception ex)
            {
                _logger?.LogError(ex, ex.Message);

                _readChannel.BasicNack(e.DeliveryTag, false, true);
            }
        }

        public Task<bool> RaiseEventAsync(IEvent e)
        {
            _logger?.LogTrace($"going to raise event -> payload: {e.Payload}");

            lock (_initializationLock)
            {
                if (!_initialized)
                {
                    _logger?.LogCritical("connector is not yet initialized, returning");

                    return Task.FromResult(false);
                }

                if (null == _writeChannel || _writeChannel.IsClosed)
                {
                    _logger?.LogWarning("write channel is closed, going to reconnect");

                    try
                    {
                        _writeChannel = _connection.CreateModel();
                    }
                    catch (System.Exception ex)
                    {
                        _logger?.LogCritical("could not create writeChannel");
                        _logger?.LogCritical(ex.Message, ex);

                        return Task.FromResult(false);
                    }
                }
            }

            byte[] data = e.Payload.PopulateMessage();

            IBasicProperties props = _writeChannel.CreateBasicProperties();
            props.MessageId = Guid.NewGuid().ToString();
            props.ContentType = "application/json";
            props.ContentEncoding = "utf8";
            props.AppId = e.EndpointId;

            _writeChannel.BasicPublish(
                e.EventType.ToString(),
                e.RoutingKey,
                true,
                props,
                data);

            return Task.FromResult(true);
        }

        public void Ack(ulong deliveryTag)
        {
            _readChannel?.BasicAck(deliveryTag, false);
        }

        public void Nack(ulong deliveryTag, bool retry)
        {
            _readChannel?.BasicNack(deliveryTag, false, retry);
        }

        public void Dispose()
        {
            _connection?.TryDispose(_logger);
            _readChannel?.TryDispose(_logger);
            _writeChannel?.TryDispose(_logger);
        }
    }
}