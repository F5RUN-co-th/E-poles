using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace E_poles.services
{
    public abstract class BaseService
    {
        private readonly IHttpClientFactory _clientFactory;
        public BaseService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> OnGet<T>(string apiEndpoints) where T : class
        {
            var request = new HttpRequestMessage(HttpMethod.Get, apiEndpoints);

            var client = _clientFactory.CreateClient("googleapi");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<T>(responseStream);
            }
            else
            {
                return default(T);
            }
        }
    }
}
