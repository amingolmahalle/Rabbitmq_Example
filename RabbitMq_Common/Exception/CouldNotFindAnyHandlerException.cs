using System.Runtime.Serialization;

namespace RabbitMq_Common.Exception
{
    public class CouldNotFindAnyHandlerException : System.Exception
    {
        public CouldNotFindAnyHandlerException()
        {
        }

        public CouldNotFindAnyHandlerException(string message) : base(message)
        {
        }

        public CouldNotFindAnyHandlerException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected CouldNotFindAnyHandlerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}