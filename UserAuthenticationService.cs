using System;
using System.Security.Cryptography;
using System.Text;
using Windows.Storage;

namespace login_full
{
	public class UserAuthenticationService
	{
		private readonly ApplicationDataContainer _localSettings;

		public UserAuthenticationService()
		{
			_localSettings = ApplicationData.Current.LocalSettings;
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

		public string GetSavedUsername()
		{
			return _localSettings.Values["Username"]?.ToString();
		}

		public string GetSavedPassword()
		{
			if (_localSettings.Values.ContainsKey("PasswordInBase64") && _localSettings.Values.ContainsKey("EntropyInBase64"))
			{
				try
				{
					var encryptedPasswordInBytes = Convert.FromBase64String(_localSettings.Values["PasswordInBase64"].ToString());
					var entropyInBytes = Convert.FromBase64String(_localSettings.Values["EntropyInBase64"].ToString());

					var decryptedPasswordInBytes = ProtectedData.Unprotect(encryptedPasswordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
					return Encoding.UTF8.GetString(decryptedPasswordInBytes);
				}
				catch (CryptographicException ex)
				{
					Console.WriteLine($"An error occurred while decrypting the password: {ex.Message}");
					return null;
				}
			}
			return null;
		}

		public void ClearSavedCredentials()
		{
			_localSettings.Values.Remove("Username");
			_localSettings.Values.Remove("PasswordInBase64");
			_localSettings.Values.Remove("EntropyInBase64");
		}
	}
}
