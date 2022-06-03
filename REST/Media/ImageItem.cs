using Newtonsoft.Json;

namespace AdaptiveImages.Welcome.REST.Media
{
    public class ImageItem : BaseRestFile
    {
        [JsonProperty("image_resolution")] public ImageResolution ImageResolution { get; set; } = new();
    }
}