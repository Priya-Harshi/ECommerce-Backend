using Ecommerce.Data.Context;
using Ecommerce.Data.Repositories.Interfaces;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Data.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ECommerceDbContext _context;
        public InventoryRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Inventory?> GetByProductIdAsync(int productId)
        {
            return await _context.Inventories
                .FirstOrDefaultAsync(x => x.ProductId == productId);
        }

        public async Task<Inventory> AddAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            return inventory;
        }

        public async Task<Inventory> UpdateAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);
            await _context.SaveChangesAsync();

            return inventory;
        }
        }
}
