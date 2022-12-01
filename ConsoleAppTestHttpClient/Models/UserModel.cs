using Newtonsoft.Json;

namespace ConsoleAppTestHttpClient.Models
{
    public sealed class UserModel
    {
        [JsonProperty]
        public int userId { get; set; }

        [JsonProperty]
        public int id { get; set; }

        [JsonProperty]
        public string title { get; set; }

        [JsonProperty]
        public string body { get; set; }
    }
}
