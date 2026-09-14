using System;
using System.Collections.Generic;
using System.Text;
using ClosedXML.Excel;
using Ecommerce.Data.Repositories.Interfaces;
using Ecommerce.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommerce.Business.Services
{
    public class BulkProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;

        public BulkProductService(
            IProductRepository productRepository,
            IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }
        public async Task<IEnumerable<Product>> ImportProductsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Excel file is empty.");
            }
            var extension = Path.GetExtension(file.FileName);

            if (!string.Equals(extension, ".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Only .xlsx Excel files are allowed.");
            }

            const int batchSize = 500;
            var products = new List<Product>();
            var allProducts = new List<Product>();

            using var stream = file.OpenReadStream();
            using var workbook = new XLWorkbook(stream);

            var worksheet = workbook.Worksheet(1);

            var rows = worksheet.RangeUsed()?.RowsUsed().Skip(1);

            if (rows == null)
            {
                throw new ArgumentException("Excel file does not contain any products.");
            }

            var errors = new List<string>();

            foreach (var row in rows)
            {
                var rowNumber = row.RowNumber();

                var name = row.Cell(1).GetString().Trim();
                var description = row.Cell(2).GetString().Trim();
                var priceText = row.Cell(3).GetString().Trim();
                var stockText = row.Cell(4).GetString().Trim();
                var activeText = row.Cell(5).GetString().Trim();

                if (string.IsNullOrWhiteSpace(name))
                {
                    errors.Add($"Row {rowNumber}: Product name is required.");
                    continue;
                }

                if (!decimal.TryParse(priceText, out decimal price))
                {
                    errors.Add($"Row {rowNumber}: Price must be a valid number.");
                    continue;
                }

                if (price < 0)
                {
                    errors.Add($"Row {rowNumber}: Price cannot be negative.");
                    continue;
                }

                if (!int.TryParse(stockText, out int stockQuantity))
                {
                    errors.Add($"Row {rowNumber}: StockQuantity must be a valid number.");
                    continue;
                }

                if (stockQuantity < 0)
                {
                    errors.Add($"Row {rowNumber}: StockQuantity cannot be negative.");
                    continue;
                }

                if (!bool.TryParse(activeText, out bool isActive))
                {
                    errors.Add($"Row {rowNumber}: IsActive must be TRUE or FALSE.");
                    continue;
                }

                allProducts.Add(new Product
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    StockQuantity = stockQuantity,
                    IsActive = isActive
                });
            }

            if (errors.Count > 0)
            {
                throw new ArgumentException(
                    "Excel validation failed:\n" +
                    string.Join("\n", errors));
            }

            if (allProducts.Count == 0)
            {
                throw new ArgumentException("No products found in Excel file.");
            }


            for (int i = 0; i < allProducts.Count; i += batchSize)
            {
                var batch = allProducts
                    .Skip(i)
                    .Take(batchSize)
                    .ToList();

                await _productRepository.AddRangeAsync(batch);
            }

            _cache.Remove("products");

            return allProducts;
        }
    }
}
