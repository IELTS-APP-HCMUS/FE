using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace login_full.API_Services
{
	public class LoginApiService
	{
		private static readonly HttpClient client = new HttpClient();
		private readonly string _baseUrl;

		public LoginApiService()
		{
			var configService = new ConfigService();
			_baseUrl = configService.GetBaseUrl();
		}
		
		public async Task<string> LoginAsync(string email, string password)
		{
			string url = $"{_baseUrl}/api/users/login";
			var loginData = new
			{
				email = email,
				password = password
			};

			string json = JsonConvert.SerializeObject(loginData);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				HttpResponseMessage response = await client.PostAsync(url, content); 

				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					return responseBody; 
				}
				else
				{
					return $"Error: {response.StatusCode}";
				}
			}
			catch (Exception ex)
			{
				return $"Exception: {ex.Message}";
			}
		}
		public async Task<string> LoginWithOAuthAsync(string idToken)
		{
			string url = $"{_baseUrl}/api/users/login";

			var oAuthData = new
			{
				id_token = idToken
			};

			string json = JsonConvert.SerializeObject(oAuthData);
			
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			System.Diagnostics.Debug.WriteLine(json);

			try
			{
				HttpResponseMessage response = await client.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					return responseBody; 
				}
				else
				{
					return $"Error: {response.StatusCode}";
				}
			}
			catch (Exception ex)
			{
				return $"Exception: {ex.Message}";
			}
		}
		public async Task<string> SignupAsync(string email, string password, string firstName, string lastName, string role)
		{
			var url = $"{_baseUrl}/api/users/signup";
			var signupData = new
			{
				email = email,
				password = password,
				first_name = firstName,
				last_name = lastName,
				role = role
			};

			string json = JsonConvert.SerializeObject(signupData);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				HttpResponseMessage response = await client.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					return responseBody; 
				}
				else
				{
					string errorContent = await response.Content.ReadAsStringAsync();
					return $"Error: {response.StatusCode} - {errorContent}";
				}
			}
			catch (Exception ex)
			{
				return $"Exception: {ex.Message}";
			}
		}
	}

}
