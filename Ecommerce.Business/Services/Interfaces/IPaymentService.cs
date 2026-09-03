using Ecommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Business.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(Payment payment);
        Task<Payment?> GetPaymentByOrderIdAsync(int orderId);
    }
}
