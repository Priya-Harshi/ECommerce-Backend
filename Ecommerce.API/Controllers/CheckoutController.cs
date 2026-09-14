using Ecommerce.API.Hubs;
using Ecommerce.Business.Services.Interfaces;
using Ecommerce.Models.DTOs.CheckOut;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckOutService _checkoutService;

        private readonly IHubContext<OrderHub> _hubContext;

        public CheckoutController(
            ICheckOutService checkoutService,
            IHubContext<OrderHub> hubContext)
        {
            _checkoutService = checkoutService;
            _hubContext = hubContext;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutRequest request)
        {
            var result =
                await _checkoutService.CheckoutAsync(request.OrderId);

            if (!result)
                return BadRequest(
                    "Checkout failed. Order may not exist or is not pending.");

            await _hubContext.Clients
                .Group($"order-{request.OrderId}")
                .SendAsync(
                    "OrderStatusChanged",
                    new
                    {
                        OrderId = request.OrderId,
                        Status = "CheckedOut"
                    });

            return Ok("Order checked out successfully.");
        }
    }
}

