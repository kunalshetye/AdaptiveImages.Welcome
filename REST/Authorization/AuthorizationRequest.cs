using EPiServer.ServiceLocation;
using Newtonsoft.Json;

namespace AdaptiveImages.Welcome.REST.Authorization
{
    public class AuthorizationRequest
    {
        [JsonProperty("grant_type")]
        public string GrantType { get; set; } = "client_credentials";

        [JsonProperty("scope")]
        public string Scope { get; set; } = "scope";

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
    }
}