using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace login_full
{
	public class GoogleAuthService
	{
		private readonly IConfiguration _configuration;
		private readonly GoogleAuthorizationCodeFlow _flow;
		private readonly string _tokenFilePath;
		private readonly string _logFilePath;
		private static bool _isInitialized = false; 

		public GoogleAuthService(IConfiguration configuration)
		{
			_configuration = configuration;

			_tokenFilePath = Path.Combine(AppContext.BaseDirectory, "GoogleToken");
			_logFilePath = Path.Combine(AppContext.BaseDirectory, "GoogleAuthServiceLog.txt");
			LogMessage($"Token directory path: {_tokenFilePath}");
			LogMessage($"Log file path: {_logFilePath}");

			if (!_isInitialized) 
			{
				LogMessage("Initializing GoogleAuthService...");
				_isInitialized = true;
			}
			try
			{
				Directory.CreateDirectory(_tokenFilePath);
				LogMessage($"Token directory created at {_tokenFilePath}");
			}
			catch (Exception ex)
			{
				LogMessage($"Error creating token directory: {ex.Message}");
			}

			_flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
			{
				ClientSecrets = new ClientSecrets
				{
					ClientId = _configuration["Authentication:Google:ClientId"],
					ClientSecret = _configuration["Authentication:Google:ClientSecret"]
				},
				Scopes = new[] { "profile", "email" },
				DataStore = new FileDataStore(_tokenFilePath, true)
			});
			LogMessage("Google Authorization Code Flow initialized.");
		}

		public async Task<UserCredential> AuthenticateAsync(CancellationToken cancellationToken)
		{
			try
			{
				LogMessage("Starting user authentication...");
				var result = await new AuthorizationCodeInstalledApp(_flow, new LocalServerCodeReceiver()).AuthorizeAsync("user", cancellationToken);
				LogMessage("User authenticated successfully.");
				return result;
			}
			catch (Exception ex)
			{
				LogMessage($"Authentication failed: {ex.Message}");
				throw new Exception("Authentication failed. Please try again.", ex);
			}
		}

		public async Task SignOutAsync()
		{
			try
			{
				LogMessage("Signing out the user...");
				if (Directory.Exists(_tokenFilePath))
				{
					Directory.Delete(_tokenFilePath, true);
					LogMessage("Token directory deleted successfully during sign-out.");
				}
			}
			catch (Exception ex)
			{
				LogMessage($"Sign out failed: {ex.Message}");
				throw new Exception("Sign out failed: " + ex.Message, ex);
			}
		}

		private void LogMessage(string message)
		{
			using (StreamWriter writer = new StreamWriter(_logFilePath, true))
			{
				writer.WriteLine($"{DateTime.Now}: {message}");
			}
		}
	}
}
