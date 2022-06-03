using Newtonsoft.Json;

namespace AdaptiveImages.Welcome.REST.Media
{
    public class Author
    {
        [JsonProperty("name")] public string Name { get; set; }
    }
}