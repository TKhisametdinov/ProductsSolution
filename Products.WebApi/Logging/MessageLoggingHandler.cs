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
        private readonly Regex _regex;

        public MessageLoggingHandler(ILogger logger)
        {
            _logger = logger;
            // regex init to use it for replacing "PhotoData":"[byte array]", substring
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
            var logMessage = PrepareMessageForLogging(message);
            await Task.Run(() =>
                _logger.Log(LogLevel.Info, $"{correlationId} - Request: {requestInfo}\r\n{logMessage}"));
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
            var logMessage = PrepareMessageForLogging(message);
            await Task.Run(() =>
                _logger.Log(LogLevel.Info, $"{correlationId} - Response: {requestInfo}\r\n{logMessage}"));
        }

        /// <summary>
        /// Prepares log message.
        /// Removes photo data bytes from message
        /// </summary>
        /// <param name="message">request message</param>
        /// <returns></returns>
        private string PrepareMessageForLogging(string message)
        {
            return _regex.Replace(message, "");
        }
    }
}