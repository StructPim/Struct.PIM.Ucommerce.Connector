using System;
using System.IO;

namespace Struct.PIM.Ucommerce.Connector.Integration
{
    public class StatusLogger
    {
        private readonly string _filePath;

        public StatusLogger(string filePath)
        {
            _filePath = filePath;
        }
        
        public DateTimeOffset? GetLastUpdate()
        {
            if (!File.Exists(_filePath))
            {
                return null;
            }

            var text = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            return DateTimeOffset.Parse(text);
        }

        public void SetLastModified(DateTimeOffset dateTime)
        {
            File.WriteAllText(_filePath, dateTime.ToString());
        }
    }
}
