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
                // If authentication fails, delete the token file to force re-authentication
                Directory.Delete(_tokenFilePath, true);
                throw new Exception("Authentication failed. Please try again.", ex);
            }
        }
    }
}

