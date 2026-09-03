using Ecommerce.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Business.Services
{
    public class OrderBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var context = scope.ServiceProvider
                    .GetRequiredService<ECommerceDbContext>();

                var pendingOrders = await context.Orders
                    .Where(o => o.Status == "Pending")
                    .ToListAsync(stoppingToken);

                // Background job processing
                foreach (var order in pendingOrders)
                {
                    // For now, we only monitor pending orders.
                    Console.WriteLine(
                        $"Background Job: Order {order.Id} is still Pending.");
                }

                await Task.Delay(
                    TimeSpan.FromMinutes(1),
                    stoppingToken);
            }
        }
    }
}
