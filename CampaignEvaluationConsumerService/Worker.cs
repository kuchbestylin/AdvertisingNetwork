using CampaignEvaluationConsumerService.Services;
using Microsoft.AspNetCore.SignalR;

namespace CampaignEvaluationConsumerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHubContext<AdvertEventsHub> _adEventsHub;

    public Worker(ILogger<Worker> logger, IHubContext<AdvertEventsHub> hubContext)
    {
        _logger = logger;
        _adEventsHub = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            try
            {
                await _adEventsHub.Clients.All.SendAsync("SendAdvertEvent", "click", new object { });
                Console.WriteLine("Sent Event");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Worker running at: {ex.Message}");
            }
            await Task.Delay(5000, stoppingToken);
        }
    }
}
