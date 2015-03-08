using System;
using Newtonsoft.Json;

namespace Streamup.Events.Ingest
{
    public class Event
    {
        [JsonProperty("event_id")]
        public string Id { get; set; }

        [JsonProperty("event_name")]
        public string Name { get; private set; }

        [JsonProperty("time")] 
        private long _time;

        public DateTime BeginsAt
        {
            get { return _time.SinceEpoch(); }
        }
    }
}