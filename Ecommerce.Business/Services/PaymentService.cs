using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Business.Services.Interfaces;
using Ecommerce.Data.Repositories.Interfaces;
using Ecommerce.Models.Entities;

namespace Ecommerce.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Payment> ProcessPaymentAsync(Payment payment)
        {
            var order = await _orderRepository.GetByIdAsync(payment.OrderId);

            if (order == null)
                throw new Exception("Order not found.");

            payment.Amount = order.TotalAmount;
            payment.Status = "Paid";

            var result = await _paymentRepository.CreateAsync(payment);

            order.Status = "Paid";
            await _orderRepository.UpdateAsync(order);

            return result;
        }

        public async Task<Payment?> GetPaymentByOrderIdAsync(int orderId)
        {
            return await _paymentRepository.GetByOrderIdAsync(orderId);
        }
    }
}
