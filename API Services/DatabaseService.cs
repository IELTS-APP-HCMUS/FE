using System;
using Npgsql;

namespace login_full.API_Services
{
	public class DatabaseService
	{
		private readonly string _connectionString;

		public DatabaseService(ConfigService config)
		{
			_connectionString = $"Host={config.GetDbHost()};Port={config.GetDbPort()};" +
								$"Database={config.GetDbName()};Username={config.GetDbUser()};" +
								$"Password={config.GetDbPassword()}";
		}

		public void ConnectToDatabase()
		{
			try
			{
				using var conn = new NpgsqlConnection(_connectionString);
				conn.Open();
				Console.WriteLine("Database connection successful.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Database connection failed: {ex.Message}");
			}
		}
	}
}
