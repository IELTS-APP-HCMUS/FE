using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;

public class ConfigService
{
	// Changed DbConfig to use Dictionary<string, string> instead of object for clarity
	public Dictionary<string, string> DbConfig { get; private set; }

	public ConfigService()
	{
		string basePath = AppContext.BaseDirectory;
		string configFilePath = Path.Combine(basePath, "API Services", "config.yaml");

		if (!File.Exists(configFilePath))
		{
			throw new FileNotFoundException($"Configuration file not found at path: {configFilePath}");
		}

		try
		{
			var yaml = File.ReadAllText(configFilePath);
			var deserializer = new Deserializer();
			var config = deserializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(yaml);
			DbConfig = config["database"];
		}
		catch (Exception ex)
		{
			throw new Exception("Failed to load or parse configuration file", ex);
		}
	}

	public string GetDbHost() => DbConfig.ContainsKey("host") ? DbConfig["host"] : throw new KeyNotFoundException("Host not found in config");
	public string GetDbPort() => DbConfig.ContainsKey("port") ? DbConfig["port"] : throw new KeyNotFoundException("Port not found in config");
	public string GetDbName() => DbConfig.ContainsKey("name") ? DbConfig["name"] : throw new KeyNotFoundException("Name not found in config");
	public string GetDbUser() => DbConfig.ContainsKey("user") ? DbConfig["user"] : throw new KeyNotFoundException("User not found in config");
	public string GetDbPassword() => DbConfig.ContainsKey("password") ? DbConfig["password"] : throw new KeyNotFoundException("Password not found in config");
}
