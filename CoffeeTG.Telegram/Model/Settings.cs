using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoffeeTG.Telegram.Model
{
    public class Settings
    {
        public Root Root { get; set; }

        public Settings()
        {
            Deserelize();
        }

        private async void Deserelize()
        {
            var jsonText = await File.ReadAllTextAsync(@"settings.json");
            Root = JsonSerializer.Deserialize<Root>(jsonText);
        }
    }

    public class Root
    {
        [JsonPropertyName("connectionString")]
        public СonnectionString СonnectionString { get; set; }

        [JsonPropertyName("connectedChat")]
        public List<ConnectedСhat> ConnectedСhat { get; set; }

        [JsonPropertyName("user")]
        public User User { get; set; }
    }

    public class СonnectionString
    {
        [JsonPropertyName("TelegramBot")]
        public string TelegramBot { get; set; }
    }

    public class ConnectedСhat
    {
        [JsonPropertyName("id")]
        public double Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class User
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("httpauthreqtype")]
        public string Httpauthreqtype { get; set; }
    }
}
