using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Business.Services.Interfaces;
using Ecommerce.Data.Repositories.Interfaces;
using Ecommerce.Models.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly IMemoryCache _cache;

        public ProductService(IProductRepository productRepository, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            if (_cache.TryGetValue("products", out IEnumerable<Product>? products))
            {
                return products!;
            }

            var result = await _productRepository.GetAllAsync();

            _cache.Set("products", result, TimeSpan.FromMinutes(5));

            return result;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            if (product.Price < 0)
            {
                throw new ArgumentException("Product price cannot be negative.");
            }

            if (product.StockQuantity < 0)
            {
                throw new ArgumentException("Stock quantity cannot be negative.");
            }

            var result = await _productRepository.AddAsync(product);

            _cache.Remove("products");

            return result;
        }

        public async Task<Product?> UpdateAsync(int id, Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return null;
            }

            if (product.Price < 0)
            {
                throw new ArgumentException("Product price cannot be negative.");
            }

            if (product.StockQuantity < 0)
            {
                throw new ArgumentException("Stock quantity cannot be negative.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.StockQuantity = product.StockQuantity;
            existingProduct.IsActive = product.IsActive;

            var result = await _productRepository.UpdateAsync(existingProduct);
            _cache.Remove("products");

            return result;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _productRepository.DeleteAsync(id);
            _cache.Remove("products");
            return result;
        }
    }
}
