using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace login_full
{
    public class UserAuthenticationService
    {
        private const string DefaultUsername = "phat1906";
        private const string DefaultPassword = "12345";
        private readonly ApplicationDataContainer _localSettings;

        public UserAuthenticationService()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
        }

        public bool ValidateCredentials(string username, string password)
        {
            return username == DefaultUsername && password == DefaultPassword;
        }

        public void SaveCredentials(string username, string password)
        {
            try
            {
                var passwordInBytes = Encoding.UTF8.GetBytes(password);
                var entropyInBytes = new byte[20];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(entropyInBytes);
                }
                var encryptedPasswordInBytes = ProtectedData.Protect(passwordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
                var encryptedPasswordBase64 = Convert.ToBase64String(encryptedPasswordInBytes);
                var entropyInBase64 = Convert.ToBase64String(entropyInBytes);

                _localSettings.Values["Username"] = username;
                _localSettings.Values["PasswordInBase64"] = encryptedPasswordBase64;
                _localSettings.Values["EntropyInBase64"] = entropyInBase64;
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"An error occurred while encrypting the password: {ex.Message}");
            }
        }

        public bool HasSavedCredentials()
        {
            return _localSettings.Values.ContainsKey("Username") &&
                   _localSettings.Values.ContainsKey("PasswordInBase64") &&
                   _localSettings.Values.ContainsKey("EntropyInBase64");
        }
    }
}
