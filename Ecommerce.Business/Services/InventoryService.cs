using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Business.Services.Interfaces;
using Ecommerce.Data.Repositories.Interfaces;
using Ecommerce.Models.Entities;

namespace Ecommerce.Business.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryService(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<Inventory?> GetByProductIdAsync(int productId)
        {
            return await _inventoryRepository.GetByProductIdAsync(productId);
        }

        public async Task<Inventory> CreateAsync(Inventory inventory)
        {
            if (inventory.Quantity < 0)
            {
                throw new ArgumentException("Inventory quantity cannot be negative.");
            }

            return await _inventoryRepository.AddAsync(inventory);
        }

        public async Task<Inventory?> UpdateAsync(int productId, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentException("Inventory quantity cannot be negative.");
            }

            var existingInventory =
                await _inventoryRepository.GetByProductIdAsync(productId);

            if (existingInventory == null)
            {
                return null;
            }

            existingInventory.Quantity = quantity;
            existingInventory.LastUpdated = DateTime.UtcNow;

            return await _inventoryRepository.UpdateAsync(existingInventory);
        }
    }
}
