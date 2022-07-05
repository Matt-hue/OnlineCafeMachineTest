using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OnlineCafe.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace OnlineCafe.HostedServices
{
    public class CafeMachineHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CafeMachineHostedService> _logger;

        public CafeMachineHostedService(
            ICafeDrinksQueue drinksQueue,
            ILogger<CafeMachineHostedService> logger, 
            IServiceProvider serviceProvider)
        {
            TaskQueue = drinksQueue;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public ICafeDrinksQueue TaskQueue { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                $"Queued Hosted Service is running.{Environment.NewLine}");

            await BackgroundProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.DequeueAsync(stoppingToken);

                try
                {
                    string message = await workItem(stoppingToken);
                    if (string.IsNullOrWhiteSpace(message))
                        continue;

                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        var cafeHubContext =
                            scope.ServiceProvider.GetRequiredService<IHubContext<CafeHub>>();

                        await cafeHubContext.Clients.All.SendAsync("ReceiveMessage", message);
                    }
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Error occurred executing background process.");
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
