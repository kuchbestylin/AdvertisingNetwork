using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AdEventingSignalRService.Services
{
    public class AdvertEventsHub : Hub
    {
        private readonly ILogger<AdvertEventsHub> _logger;
        private HttpClient _httpClient = new HttpClient();

        public AdvertEventsHub(ILogger<AdvertEventsHub> logger)
        {
            _logger = logger;
        }

        public async Task SendAdvertEvent(string name, double count, int id)
        {
            Console.WriteLine($"::: {name} => {count} => {id}");

            //Sends Metric/EventData to the respective handler
            _httpClient.BaseAddress = new Uri("https://localhost:10000");
            await _httpClient.PostAsJsonAsync($"/reporting/metrics/{id}", new EventData(name, count, id));

            // Broadcast the event to all clients
            await Clients.All.SendAsync("ReceiveEvent", name, count, id);
        }

        record EventData(string name, double count, int id);
    }
}
