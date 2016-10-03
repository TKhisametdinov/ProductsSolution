using System.Net.Http;
using System.Threading.Tasks;

namespace Products.Shared.Interfaces
{
    public interface IHttpClientHelper
    {
        Task<HttpResponseMessage> GetAsync(string urlWithParams);

        Task<HttpResponseMessage> DeleteAsync(string urlWithParams);

        Task<HttpResponseMessage> PostAsync(string urlWithParams, string content);

        Task<HttpResponseMessage> PutAsync(string urlWithParams, string content);
    }
}
