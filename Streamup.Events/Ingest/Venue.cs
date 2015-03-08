using Newtonsoft.Json;

namespace Streamup.Events.Ingest
{
    public class Venue
    {
        [JsonProperty("venue_name")]
        public string Name { get; private set; }

        [JsonProperty("lon")]
        public double Longitude { get; private set; }

        [JsonProperty("lat")]
        public double Latitude { get; private set; }
    }
}