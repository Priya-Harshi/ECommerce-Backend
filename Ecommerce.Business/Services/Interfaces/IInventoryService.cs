using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Models.Entities;

namespace Ecommerce.Business.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<Inventory?> GetByProductIdAsync(int productId);

        Task<Inventory> CreateAsync(Inventory inventory);

        Task<Inventory?> UpdateAsync(int productId, int quantity);
    }
}
