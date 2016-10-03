using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;

namespace Products.WebApi.Logging
{
    /// <summary>
    /// Logging message handler implementation
    /// </summary>
    public class MessageLoggingHandler : MessageHandler
    {
        private readonly ILogger _logger;
        private Regex _regex;

        public MessageLoggingHandler(ILogger logger)
        {
            _logger = logger;
            _regex = new Regex("\"PhotoData\":\".*\",");
        }

        /// <summary>
        /// Logs incoming request message
        /// </summary>
        /// <param name="correlationId">correlation id</param>
        /// <param name="requestInfo">request info</param>
        /// <param name="message">request message</param>
        /// <returns>task handle for logging operation</returns>
        protected override async Task IncommingMessageAsync(string correlationId, string requestInfo, string message)
        {
            var mes = _regex.Replace(message, "");
            await Task.Run(() =>
                _logger.Log(LogLevel.Info, $"{correlationId} - Request: {requestInfo}\r\n{mes}"));
        }

        /// <summary>
        /// Logs outgoing response message
        /// </summary>
        /// <param name="correlationId">correlation id</param>
        /// <param name="requestInfo">request info</param>
        /// <param name="message">response message</param>
        /// <returns>task handle for logging operation</returns>
        protected override async Task OutgoingMessageAsync(string correlationId, string requestInfo, string message)
        {
            var mes = _regex.Replace(message, "");
            await Task.Run(() =>
                _logger.Log(LogLevel.Info, $"{correlationId} - Response: {requestInfo}\r\n{mes}"));
        }
    }
}