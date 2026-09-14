using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Models.Entities;

namespace Ecommerce.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Product>> AddRangeAsync(IEnumerable<Product> products);
    }
}
