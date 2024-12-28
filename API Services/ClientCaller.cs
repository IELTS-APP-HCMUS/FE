using login_full.Context;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace login_full.API_Services
{
    public class ClientCaller
    {
        private readonly HttpClient _httpClient;

        public ClientCaller()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080")
            };
            string accessToken = GlobalState.Instance.AccessToken;

            // Thêm AccessToken vào Header nếu có
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            return await _httpClient.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, HttpContent content)
        {
            return await _httpClient.PostAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> PatchAsync(string endpoint, HttpContent content)
        {
            return await _httpClient.PatchAsync(endpoint, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await _httpClient.DeleteAsync(endpoint);
        }

        public static StringContent GetContent<T>(T item)
        {
            string json = JsonSerializer.Serialize(item);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
