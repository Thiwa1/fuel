using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace WpfApp1.Services
{
    public class DatabaseConfigService
    {
        private const string ConnectionStringName = "DefaultConnection";
        private const string AppSettingsFile = "appsettings.json";

        public MySqlConnectionStringBuilder GetConnectionStringBuilder()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettingsFile, optional: true, reloadOnChange: true);

            var config = builder.Build();
            var connectionString = config.GetConnectionString(ConnectionStringName);

            if (string.IsNullOrEmpty(connectionString))
            {
                // Return default if empty
                return new MySqlConnectionStringBuilder
                {
                    Server = "localhost",
                    Database = "pay_bill",
                    UserID = "root",
                    Password = ""
                };
            }

            return new MySqlConnectionStringBuilder(connectionString);
        }

        public void SaveConnectionString(string server, string database, string userId, string password)
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Server = server,
                Database = database,
                UserID = userId,
                Password = password
            };

            string newConnectionString = builder.ToString();

            // Load existing JSON
            string jsonString = File.Exists(AppSettingsFile) ? File.ReadAllText(AppSettingsFile) : "{}";
            var jsonNode = JsonNode.Parse(jsonString);

            if (jsonNode == null)
            {
                jsonNode = new JsonObject();
            }

            // Navigate to ConnectionStrings:DefaultConnection
            if (jsonNode["ConnectionStrings"] == null)
            {
                jsonNode["ConnectionStrings"] = new JsonObject();
            }

            jsonNode["ConnectionStrings"]![ConnectionStringName] = newConnectionString;

            // Write back to file
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(AppSettingsFile, jsonNode.ToJsonString(options));
        }
    }
}
