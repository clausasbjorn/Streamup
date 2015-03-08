using Newtonsoft.Json;

namespace Streamup.Events.Ingest
{
    public class Group
    {
        [JsonProperty("group_id")]
        public long Id { get; set; }

        [JsonProperty("group_name")]
        public string Name { get; private set; }

        [JsonProperty("group_country")]
        public string Country { get; private set; }

        [JsonProperty("group_topics")]
        public Topic[] Topics { get; private set; }
    }
}