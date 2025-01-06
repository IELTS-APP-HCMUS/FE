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
         
			var request = new HttpRequestMessage(HttpMethod.Get, endpoint);

			// Log Request Details
			LogRequest(request);
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

		private void LogRequest(HttpRequestMessage request)
		{
			// Start building the cURL command
			var curlCommand = new StringBuilder("curl -X ");

			// Add HTTP method
			curlCommand.Append($"{request.Method} ");

			// Ensure the full URL is used (BaseAddress + Relative URI)
			var fullUrl = new Uri(_httpClient.BaseAddress, request.RequestUri);
			curlCommand.Append($"\"{fullUrl}\" ");

			// Add headers
			foreach (var header in request.Headers)
			{
				curlCommand.Append($"-H \"{header.Key}: {string.Join(", ", header.Value)}\" ");
			}

			// Add Authorization header if present
			if (_httpClient.DefaultRequestHeaders.Authorization != null)
			{
				curlCommand.Append($"-H \"Authorization: Bearer {_httpClient.DefaultRequestHeaders.Authorization.Parameter}\" ");
			}

			// Add content (body) if available
			if (request.Content != null)
			{
				string body = request.Content.ReadAsStringAsync().Result; // Use async in production
				curlCommand.Append($"-d '{body}' ");
			}

			// Set content type to JSON
			curlCommand.Append("-H \"Content-Type: application/json\" ");

			// Log the generated cURL command
			System.Diagnostics.Debug.WriteLine("cURL Command:");
			System.Diagnostics.Debug.WriteLine(curlCommand.ToString());
		}

	}
}
