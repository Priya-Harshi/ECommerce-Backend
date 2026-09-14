using Ecommerce.Business.Services.Interfaces;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var userIdClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized();
            }

            order.UserId = userId;

            var result = await _orderService.CreateOrderAsync(order);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);

            if (result == null)
                return NotFound();

            if (User.IsInRole("Customer"))
            {
                var userIdClaim =
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized();
                }

                if (result.UserId != userId)
                {
                    return Forbid();
                }
            }

            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetOrdersByUser(int userId)
        {
            if (User.IsInRole("Customer"))
            {
                var userIdClaim =
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!int.TryParse(userIdClaim, out int loggedInUserId))
                {
                    return Unauthorized();
                }

                if (userId != loggedInUserId)
                {
                    return Forbid();
                }
            }

            var result =
                await _orderService.GetOrdersByUserIdAsync(userId);

            return Ok(result);
        }
    }
}
