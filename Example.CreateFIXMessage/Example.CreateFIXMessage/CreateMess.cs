using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.CreateFIXMessage
{
    public class CreateMess
    {
        private const char SOH = (char)124; 

        private readonly Dictionary<int, string> _fields = new Dictionary<int, string>();

        public void AddField(int tag, string value)
        {
            _fields[tag] = value;
        }

        public string CreateMessage(string beginString, string senderCompId, string targetCompId, string messageType, int sequenceNumber)
        {
            AddField(8, beginString); 
            AddField(35, messageType);
            AddField(49, senderCompId); 
            AddField(56, targetCompId);
            AddField(34, sequenceNumber.ToString());
            AddField(52, DateTime.UtcNow.ToString("yyyyMMdd-HH:mm:ss.fff"));

            StringBuilder sb = new StringBuilder();
            foreach (var field in _fields.OrderBy(k => k.Key)) 
            {
                sb.Append($"{field.Key}={field.Value}{SOH}");
            }

            int bodyLength = sb.Length - 2; 
            sb.Insert(0, $"{9}={bodyLength}{SOH}"); 

            int checksum = CalculateChecksum(sb.ToString());
            sb.Append($"{10}={checksum}");

            return sb.ToString();
        }

        private int CalculateChecksum(string message)
        {
            int sum = 0;
            foreach (char c in message)
            {
                sum += (byte)c;
            }
            return sum % 256; 
        }
    }
}
