using Newtonsoft.Json;

namespace AdaptiveImages.Welcome.REST.Media
{
    public class Source
    {
        [JsonProperty("name")] public string Name { get; set; }
    }
}