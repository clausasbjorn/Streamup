using System;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;

namespace Streamup.Push
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init event hub processor for reading Stream Analytics job output
            var eventHubConnectionString = ConfigurationManager.AppSettings["Streamup.EventHub.ConnectionString"];
            var eventHubName = ConfigurationManager.AppSettings["Streamup.EventHub.Name"];

            var storageConnectionString = ConfigurationManager.AppSettings["Streamup.Storage.ConnectionString"];

            var eventProcessorHostName = Guid.NewGuid().ToString();
            var eventProcessorHost = new EventProcessorHost(
                eventProcessorHostName, 
                eventHubName, 
                EventHubConsumerGroup.DefaultGroupName, 
                eventHubConnectionString, 
                storageConnectionString
            );
            
            eventProcessorHost.RegisterEventProcessorAsync<StatsProcessor>().Wait();
            
            Console.ReadLine();
        }
    }
}
