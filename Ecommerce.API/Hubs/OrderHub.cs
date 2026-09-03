using Microsoft.AspNetCore.SignalR;

namespace Ecommerce.API.Hubs
{
    public class OrderHub : Hub
    {
        public async Task JoinOrderGroup(int orderId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                $"order-{orderId}");
        }
    }
}