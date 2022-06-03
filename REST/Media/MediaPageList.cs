using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdaptiveImages.Welcome.REST.Media
{
    public class MediaPageList
    {
        [JsonProperty("data")] public List<MediaItem> Assets { get; set; }

        [JsonProperty("pagination")] public Pagination Pagination { get; set; }
    }
}