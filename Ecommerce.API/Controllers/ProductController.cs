using Ecommerce.Business.Services.Interfaces;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.Business.Services;
using Microsoft.AspNetCore.Http;

namespace ECommerce.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly BulkProductService _bulkProductService;
        public ProductController(
        IProductService productService,
        BulkProductService bulkProductService)
        {
            _productService = productService;
            _bulkProductService = bulkProductService;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);
        }

        // GET: api/Product/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        // POST: api/Product
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                var createdProduct = await _productService.CreateAsync(product);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = createdProduct.Id },
                    createdProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Product/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            try
            {
                var updatedProduct =
                    await _productService.UpdateAsync(id, product);

                if (updatedProduct == null)
                {
                    return NotFound("Product not found.");
                }

                return Ok(updatedProduct);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Product/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _productService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound("Product not found.");
            }

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("bulk-upload")]
        public async Task<IActionResult> BulkUpload(IFormFile file)
        {
            try
            {
                var products = await _bulkProductService.ImportProductsAsync(file);

                return Ok(new
                {
                    Message = "Products uploaded successfully.",
                    Count = products.Count()
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
