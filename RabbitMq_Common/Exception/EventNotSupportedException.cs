using System.Runtime.Serialization;

namespace RabbitMq_Common.Exception
{
    public class EventNotSupportedException : System.Exception
    {
        public EventNotSupportedException()
        {
        }

        public EventNotSupportedException(string message) : base(message)
        {
        }

        public EventNotSupportedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected EventNotSupportedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}