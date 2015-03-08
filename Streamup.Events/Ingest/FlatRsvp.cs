using System;

namespace Streamup.Events.Ingest
{
    public class FlatRsvp
    {
        public bool IsGoing { get; set; }

        public DateTime RepliedAt { get; set; }

        public int RepliedMinutesBeforeEvent { get; set; }
        
        public long GroupId { get; set; }

        public string GroupName { get; set; }

        public string GroupCountry { get; set; }

        public string EventId { get; set; }

        public string EventName { get; set; }
    }
}
