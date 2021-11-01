
using NLog;
using System.Text.Json;


namespace DemoApplication1.Models.Entities.Concretes
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();
        
        public string Message { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
    
        }
    }
}
