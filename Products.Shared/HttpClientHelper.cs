using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Products.Shared.Interfaces;

namespace Products.Shared
{
    public class HttpClientHelper : IHttpClientHelper
    {
        public async Task<HttpResponseMessage> GetAsync(string urlWithParams)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetAsync(urlWithParams);
            }
        }

        public async Task<HttpResponseMessage> DeleteAsync(string urlWithParams)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.DeleteAsync(urlWithParams);
            }
        }

        public async Task<HttpResponseMessage> PostAsync(string urlWithParams, string content)
        {
            using (var httpClient = new HttpClient())
            {
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                return await httpClient.PostAsync(urlWithParams, stringContent);
            }
        }

        public async Task<HttpResponseMessage> PutAsync(string urlWithParams, string content)
        {
            using (var httpClient = new HttpClient())
            {
                var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                return await httpClient.PutAsync(urlWithParams, stringContent);
            }
        }
    }
}
