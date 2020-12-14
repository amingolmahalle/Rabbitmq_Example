using Microsoft.Extensions.Logging;
using RabbitMq_Consumer.Extension;

namespace RabbitMq_Consumer.Handlers.SendEmail
{
    public class SendEmailValidation
    {
        public static bool ValidationCommand(SendEmailCommand command,ILogger logger)
        {
            var isValid = true;

            if (string.IsNullOrWhiteSpace(command?.Email))
            {
                logger?.LogError($"invalid Email Address: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(command?.Message))
            {
                logger?.LogError($"invalid Email Message: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(command?.Subject))
            {
                logger?.LogError($"invalid Email Subject: empty or null, can not process nack/no-retry");

                isValid = false;
            }

            if (command?.Email?.IsValidEmail() != true)
            {
                logger?.LogError($"invalid Email Address: regular expression, can not process nack/no-retry");
                
                isValid = false;
            }

            return isValid;
        }
    }
}