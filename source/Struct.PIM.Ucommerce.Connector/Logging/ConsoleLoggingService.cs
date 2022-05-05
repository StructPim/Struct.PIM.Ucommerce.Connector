using System;
using Ucommerce.Infrastructure.Logging;
using Ucommerce.Infrastructure.Logging.Capturing;

namespace Struct.PIM.Ucommerce.Connector.Logging
{
    public class ConsoleLoggingService : ILoggingService
    {
        public void Debug<T>(string message)
        {
            Console.WriteLine(message);
        }

        public void Debug<T>(string messageTemplate, params object[] propertyValues)
        {
            MessageTemplateParser.TryParse(messageTemplate, propertyValues, out var message);
            Debug<T>(message);
        }

        public void Information<T>(string message)
        {
            Console.WriteLine(message);
        }

        public void Information<T>(string messageTemplate, params object[] propertyValues)
        {
            MessageTemplateParser.TryParse(messageTemplate, propertyValues, out var message);
            Information<T>(message);
        }

        public void Error<T>(Exception exception)
        {
            Console.WriteLine(exception);
        }

        public void Error<T>(Exception exception, string message)
        {
            Console.WriteLine(message);
            Console.WriteLine(exception);
        }

        public void Error<T>(Exception exception, string messageTemplate, params object[] propertyValues)
        {
            MessageTemplateParser.TryParse(messageTemplate, propertyValues, out var message);
            Error<T>(exception, message);
        }
    }
}
