using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Models.Entities;

namespace Ecommerce.Data.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetByOrderIdAsync(int OrderId);
    }
}
