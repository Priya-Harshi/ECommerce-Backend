using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Models.Entities;

namespace Ecommerce.Data.Repositories.Interfaces
{
    public interface IInventoryRepository
    {
        Task<Inventory?> GetByProductIdAsync(int productId);
        Task<Inventory> AddAsync(Inventory inventory);
        Task<Inventory> UpdateAsync(Inventory inventory);
    }
}
