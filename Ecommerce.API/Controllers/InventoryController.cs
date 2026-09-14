using Ecommerce.Business.Services.Interfaces;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var inventory =
                await _inventoryService.GetByProductIdAsync(productId);

            if (inventory == null)
            {
                return NotFound("Inventory not found.");
            }

            return Ok(inventory);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Inventory inventory)
        {
            try
            {
                var created =
                    await _inventoryService.CreateAsync(inventory);

                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{productId}")]
        public async Task<IActionResult> Update(
            int productId,
            [FromBody] int quantity)
        {
            try
            {
                var updated =
                    await _inventoryService.UpdateAsync(
                        productId, quantity);

                if (updated == null)
                {
                    return NotFound("Inventory not found.");
                }

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

