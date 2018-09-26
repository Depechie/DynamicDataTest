using Newtonsoft.Json;

namespace StravaNetStandard.Models
{
    public class AccessToken
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
    }
}
