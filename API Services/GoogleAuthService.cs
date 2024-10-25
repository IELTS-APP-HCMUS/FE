using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using static Google.Apis.Requests.BatchRequest;

namespace login_full
{

    public class GoogleAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly GoogleAuthorizationCodeFlow _flow;
        private readonly string _tokenFilePath;

        public GoogleAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
			_tokenFilePath = Path.Combine(AppContext.BaseDirectory, "GoogleAuthToken");
			System.IO.Directory.CreateDirectory(_tokenFilePath);

			//if (!System.IO.Directory.Exists(_tokenFilePath))
			//{
				
			//}
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
        }

        public async Task<UserCredential> AuthenticateAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await new AuthorizationCodeInstalledApp(_flow, new LocalServerCodeReceiver()).AuthorizeAsync("user", cancellationToken);
                return result;
            }
            catch (Exception ex)
            {

                Directory.Delete(_tokenFilePath, true);
                throw new Exception("Authentication failed. Please try again.", ex);
            }
        }
		public async Task SignOutAsync()
		{
			try
			{
				if (Directory.Exists(_tokenFilePath))
				{
					Directory.Delete(_tokenFilePath, true); 
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Sign out failed: " + ex.Message);
			}
		}

	}
}

