using System.Runtime.Serialization;

namespace RabbitMq_Common.Exception
{
    public class HandlingMinimumRequirementsFailedException : System.Exception
    {
        public HandlingMinimumRequirementsFailedException()
        {
        }

        public HandlingMinimumRequirementsFailedException(string message) : base(message)
        {
        }

        public HandlingMinimumRequirementsFailedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected HandlingMinimumRequirementsFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}