using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Products.WebApi.Logging
{
    /// <summary>
    /// Base abstract message handler for logging request (response) messages
    /// </summary>
    public abstract class MessageHandler : DelegatingHandler
    {
        /// <summary>
        /// Adding cross-cutting loggin concern to Web API pipeline
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // correlation id to combine the request with the response
            var corrId = $"{DateTime.Now.Ticks}{Thread.CurrentThread.ManagedThreadId}";
            var requestInfo = $"{request.Method} {request.RequestUri}";

            var requestMessage = await request.Content.ReadAsStringAsync();

            await IncommingMessageAsync(corrId, requestInfo, requestMessage);

            var response = await base.SendAsync(request, cancellationToken);

            string responseMessage;

            if (response.IsSuccessStatusCode && response.Content != null)
                responseMessage = await response.Content.ReadAsStringAsync();
            else
                responseMessage = response.ReasonPhrase;

            await OutgoingMessageAsync(corrId, requestInfo, responseMessage);

            return response;
        }

        protected abstract Task IncommingMessageAsync(string correlationId, string requestInfo, string message);

        protected abstract Task OutgoingMessageAsync(string correlationId, string requestInfo, string message);
    }
}