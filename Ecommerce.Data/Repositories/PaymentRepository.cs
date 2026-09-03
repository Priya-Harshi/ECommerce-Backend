using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Data.Context;
using Ecommerce.Data.Repositories.Interfaces;
using Ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ECommerceDbContext _context;

        public PaymentRepository(ECommerceDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }
    }
}
