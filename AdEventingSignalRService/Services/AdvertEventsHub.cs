using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdEventingSignalRService.Services
{
    public class AdvertEventsHub : Hub
    {
        private readonly ILogger<AdvertEventsHub> _logger;

        public AdvertEventsHub(ILogger<AdvertEventsHub> logger)
        {
            _logger = logger;
        }

        public async Task SendAdvertEvent(string eventType, object eventData)
        {
            // Log the event received from the client
            _logger.LogInformation($"Received event from client. Type: {eventType}, Data: {eventData}");
            Console.WriteLine($"Received event from client. Type: {eventType}, Data: {eventData}");
            // Broadcast the event to all clients
            await Clients.All.SendAsync("ReceiveEvent", eventType, eventData);
        }
    }
}
