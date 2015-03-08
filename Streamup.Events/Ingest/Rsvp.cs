using System;
using Newtonsoft.Json;

namespace Streamup.Events.Ingest
{
    public class Rsvp
    {
        [JsonProperty("response")]
        private string _response;

        public bool IsGoing
        {
            get { return _response != null && _response != "no"; }
        }

        [JsonProperty("mtime")] 
        private long _mTime;

        public DateTime RepliedAt
        {
            get { return _mTime.SinceEpoch(); }
        }

        [JsonProperty("event")]
        public Event Event { get; private set; }
        
        [JsonProperty("venue")]
        public Venue Venue { get; private set; }
        
        [JsonProperty("group")]
        public Group Group { get; private set; }
    }
}
