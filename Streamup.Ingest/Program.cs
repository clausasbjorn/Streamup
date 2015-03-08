using System;
using System.Configuration;
using System.Text;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Streamup.Events.Ingest;
using WebSocket4Net;

namespace Streamup.Ingest
{
    class Program
    {
        private static WebSocket _websocket;
        private static EventHubClient _eventHub;

        static void _websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var rsvp = JsonConvert.DeserializeObject<Rsvp>(e.Message);

            var flat = new FlatRsvp()
            {
                IsGoing = rsvp.IsGoing,
                RepliedAt = rsvp.RepliedAt,
                RepliedMinutesBeforeEvent = (int) rsvp.Event.BeginsAt.Subtract(rsvp.RepliedAt).TotalMinutes,
                GroupId = rsvp.Group.Id,
                GroupName = rsvp.Group.Name,
                GroupCountry = rsvp.Group.Country,
                EventId = rsvp.Event.Id,
                EventName = rsvp.Event.Name
            };

            var serialized = JsonConvert.SerializeObject(flat);

            _eventHub.Send(new EventData(Encoding.UTF8.GetBytes(serialized)));
        }

        static void Main(string[] args)
        {
            // Init event hub to write events to
            var connectionString = ConfigurationManager.AppSettings["Streamup.EventHub.ConnectionString"];
            var eventHubName = ConfigurationManager.AppSettings["Streamup.EventHub.Name"];
            _eventHub = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);

            // Init websocket for fetching Meetup data
            _websocket = new WebSocket("ws://stream.meetup.com/2/rsvps");
            _websocket.MessageReceived += _websocket_MessageReceived;
            _websocket.Open();

            Console.ReadLine();
        }
    }
}
