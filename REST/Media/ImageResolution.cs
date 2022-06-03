using Newtonsoft.Json;

namespace AdaptiveImages.Welcome.REST.Media
{
    public class ImageResolution
    {
        [JsonProperty("height")] public int Height { get; set; }

        [JsonProperty("width")] public int Width { get; set; }
    }
}