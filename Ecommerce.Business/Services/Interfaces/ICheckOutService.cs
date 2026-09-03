using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Business.Services.Interfaces
{
    public interface ICheckOutService
    {
        Task<bool> CheckoutAsync(int orderId);
    }
}
