using System.Text;
using System.Web.Http.ExceptionHandling;
using NLog;

namespace Products.WebApi.Logging
{
    /// <summary>
    /// Custom exception logger using Nlog
    /// </summary>
    public class NLogExceptionLogger : ExceptionLogger
    {
        private static ILogger _logger;

        public NLogExceptionLogger(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Logs the exception synchronously
        /// </summary>
        /// <param name="context">The exception logger context</param>
        public override void Log(ExceptionLoggerContext context)
        {
            _logger.Log(LogLevel.Error, context.Exception, ContextToString(context));
        }

        /// <summary>
        /// Builds log message string from exception logger context
        /// </summary>
        /// <param name="context">The exception logger context</param>
        /// <returns>Log message string</returns>
        private static string ContextToString(ExceptionLoggerContext context)
        {
            var message = new StringBuilder();

            var request = context.Request;
            if (request.Method != null)
            {
                message.Append(request.Method);
            }

            if (request.RequestUri != null)
            {
                message.Append(" ").Append(request.RequestUri);
            }

            if (context.Exception != null)
            {
                message.Append(" msg: ").Append(context.Exception.Message);
                message.Append(" inner: ").Append(context.Exception.InnerException?.Message);
            }

            return message.ToString();
        }
    }
}