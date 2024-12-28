using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace login_full.API_Services
{
	public class LoginApiService
	{
		//private static readonly HttpClient client = new HttpClient();
		private ClientCaller _clientCaller = new();

        public async Task<string> LoginAsync(string email, string password)
		{
			var loginData = new
			{
				email = email,
				password = password
			};

			var content = ClientCaller.GetContent(loginData);

            try
            {
				HttpResponseMessage response = await _clientCaller.PostAsync("api/users/login", content); 

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
			//string url = $"{_baseUrl}/api/users/login";

			var oAuthData = new
			{
				id_token = idToken
			};

			//string json = JsonConvert.SerializeObject(oAuthData);
			
			//var content = new StringContent(json, Encoding.UTF8, "application/json");
			//System.Diagnostics.Debug.WriteLine(json);
			var content = ClientCaller.GetContent(oAuthData);

            try
			{
				HttpResponseMessage response = await _clientCaller.PostAsync("api/users/login", content);

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
			var signupData = new
			{
				email = email,
				password = password,
				first_name = firstName,
				last_name = lastName,
				role = role
			};

			var content = ClientCaller.GetContent(signupData);

            try
			{
				HttpResponseMessage response = await _clientCaller.PostAsync("api/users/login", content);

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
