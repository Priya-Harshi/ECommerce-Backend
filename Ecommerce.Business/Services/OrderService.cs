using Ecommerce.Business.Services;
using Ecommerce.Business.Services.Interfaces;
using Ecommerce.Data.Repositories.Interfaces;
using Ecommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            return await _orderRepository.CreateAsync(order);
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _orderRepository.GetByUserIdAsync(userId);
        }
    }
}
