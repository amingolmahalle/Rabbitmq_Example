using Microsoft.Extensions.Logging;
using RabbitMq_Consumer.Extension;

namespace RabbitMq_Consumer.Handlers.SendSms
{
    public class SendSmsValidation
    {
        public static bool ValidationCommand(SendSmsCommand command,ILogger logger)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(command?.Mobile))
            {
                logger?.LogError($"invalid Mobile Number: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(command?.Message))
            {
                logger?.LogError($"invalid Sms Message: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            if (command?.Mobile?.IsValidMobileNumber() != true)
            {
                logger?.LogError($"invalid Mobile Number: regular expression, can not process nack/no-retry");
                
                isValid = false;
            }

            return isValid;
        }
    }
}