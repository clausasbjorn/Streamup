using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Streamup.Events.Push;

namespace Streamup.Push
{
    public class StatsProcessor : IEventProcessor
    {
        private string _dashboardUri;
        private Stopwatch _checkpointStopWatch;

        public StatsProcessor()
        {
            _dashboardUri = ConfigurationManager.AppSettings["Streamup.Dashboard.Uri"];
        }

        async Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
        {
            if (reason == CloseReason.Shutdown) { await context.CheckpointAsync(); }
        }

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            _checkpointStopWatch = new Stopwatch();
            _checkpointStopWatch.Start();
            return Task.FromResult<object>(null);
        }

        async Task IEventProcessor.ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (EventData eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.GetBytes());

                // Extract top 10 by RsvpCount
                var groups = JsonConvert.DeserializeObject<List<GroupStats>>(data);
                var top = groups.OrderByDescending(g => g.RsvpCount).Take(10).Select(g => new {
                    Group = String.Format("{0} ({1})", g.GroupName, g.GroupCountry),
                    Count = g.RsvpCount
                }).ToList();

                // Build and post CSV data to Dashboard
                //var csv = new StringBuilder();
                //csv.AppendLine("Group,RSVPs");

                //foreach (var group in top)
                //    csv.AppendLine(String.Format("{0},{1}", group.Group.Replace(",", ""), group.Count));

                //Post(_dashboardUri, csv.ToString());
            
                // Print top 10 to console
                foreach (var group in top)
                    Console.WriteLine("{0, -10}{1}", group.Count, group.Group);

                Console.WriteLine();
            }

            if (_checkpointStopWatch.Elapsed > TimeSpan.FromMinutes(5))
            {
                await context.CheckpointAsync();
                _checkpointStopWatch.Restart();
            }
        }

        private static void Post(string url, string payload)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "text/csv");
                client.UploadData(url, Encoding.UTF8.GetBytes(payload));
            }
        }
    }
}
