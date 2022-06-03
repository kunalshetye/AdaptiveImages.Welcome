using Newtonsoft.Json;

namespace AdaptiveImages.Welcome.REST.Media
{
    public class Group
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }
    }
}