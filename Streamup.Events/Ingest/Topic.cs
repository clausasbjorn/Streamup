using Newtonsoft.Json;

namespace Streamup.Events.Ingest
{
    public class Topic
    {
        [JsonProperty("urlkey")]
        public string Key { get; private set; }

        [JsonProperty("topic_name")]
        public string Name { get; private set; }
    }
}