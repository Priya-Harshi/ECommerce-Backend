using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Business.Services.Interfaces;
using Ecommerce.Data.Repositories.Interfaces;
using Ecommerce.Models.Entities;

namespace Ecommerce.Business.Services
{
    public class CheckoutService : ICheckOutService
    {
        private readonly IOrderRepository _orderRepository;
         public CheckoutService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<bool> CheckoutAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                return false;

            if (order.Status != "Pending")
                return false;

            order.Status = "CheckedOut";

            await _orderRepository.UpdateAsync(order);

            return true;
        }
    }
}
