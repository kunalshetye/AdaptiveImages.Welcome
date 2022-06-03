using Newtonsoft.Json;

namespace AdaptiveImages.Welcome.REST.Media
{
    public class Content
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("value")] public string Value { get; set; }
    }
}