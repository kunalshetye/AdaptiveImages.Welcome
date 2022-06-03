using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdaptiveImages.Welcome.REST.Media
{
    public class Label
    {
        [JsonProperty("group")] public Group Group { get; set; }

        [JsonProperty("values")] public List<GroupValue> Values { get; set; }
    }
}