using System;
using System.Collections.Generic;
using System.Text;
using Ecommerce.Models.DTOs.Auth;

namespace Ecommerce.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterRequest request);
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
