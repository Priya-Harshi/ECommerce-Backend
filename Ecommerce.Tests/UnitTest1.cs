using Ecommerce.Business.Services;
using Ecommerce.Data.Repositories.Interfaces;
using Ecommerce.Models.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace ECommerce.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task CreateAsync_NegativePrice_ThrowsArgumentException()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var service = new ProductService(null!, cache);

            var product = new Product
            {
                Name = "Test Product",
                Price = -100,
                StockQuantity = 10,
                IsActive = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateAsync(product));
        }

        [Fact]
        public async Task CreateAsync_NegativeStock_ThrowsArgumentException()
        {
            // Arrange
            var cache = new MemoryCache(new MemoryCacheOptions());
            var service = new ProductService(null!, cache);

            var product = new Product
            {
                Name = "Test Product",
                Price = 1000,
                StockQuantity = -5,
                IsActive = true
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(
                () => service.CreateAsync(product));
        }

        [Fact]
        public async Task CreateAsync_ValidProduct_ReturnsProduct()
        {
            // Arrange
            var fakeRepository = new FakeProductRepository();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var service = new ProductService(fakeRepository, cache);

            var product = new Product
            {
                Name = "Test Laptop",
                Price = 55000,
                StockQuantity = 10,
                IsActive = true
            };

            // Act
            var result = await service.CreateAsync(product);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Laptop", result.Name);
            Assert.Equal(55000, result.Price);
            Assert.Equal(10, result.StockQuantity);
        }
        [Fact]
        public async Task UpdateAsync_ExistingProduct_UpdatesProduct()
        {
            // Arrange
            var existingProduct = new Product
            {
                Id = 1,
                Name = "Old Laptop",
                Price = 40000,
                StockQuantity = 5,
                IsActive = true
            };

            var fakeRepository = new FakeProductRepository
            {
                ExistingProduct = existingProduct
            };

            var cache = new MemoryCache(new MemoryCacheOptions());
            var service = new ProductService(fakeRepository, cache);

            var updatedProduct = new Product
            {
                Name = "New Laptop",
                Price = 55000,
                StockQuantity = 10,
                IsActive = true
            };

            // Act
            var result = await service.UpdateAsync(1, updatedProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Laptop", result.Name);
            Assert.Equal(55000, result.Price);
            Assert.Equal(10, result.StockQuantity);
        }
        [Fact]
        public async Task GetOrderByIdAsync_ExistingOrder_ReturnsOrder()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalAmount = 55000,
                Status = "Pending"
            };

            var fakeRepository = new FakeOrderRepository
            {
                ExistingOrder = order
            };

            var service = new OrderService(fakeRepository);

            // Act
            var result = await service.GetOrderByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal(55000, result.TotalAmount);
            Assert.Equal("Pending", result.Status);
        }
        [Fact]
        public async Task CreateOrderAsync_ValidOrder_ReturnsOrder()
        {
            // Arrange
            var fakeRepository = new FakeOrderRepository();
            var service = new OrderService(fakeRepository);

            var order = new Order
            {
                UserId = 1,
                TotalAmount = 55000,
                Status = "Pending"
            };

            // Act
            var result = await service.CreateOrderAsync(order);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.UserId);
            Assert.Equal(55000, result.TotalAmount);
            Assert.Equal("Pending", result.Status);
        }
        [Fact]
        public async Task CheckoutAsync_PendingOrder_ReturnsTrue()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalAmount = 55000,
                Status = "Pending"
            };

            var fakeRepository = new FakeOrderRepository
            {
                ExistingOrder = order
            };

            var service = new CheckoutService(fakeRepository);

            // Act
            var result = await service.CheckoutAsync(1);

            // Assert
            Assert.True(result);
            Assert.Equal("CheckedOut", order.Status);
        }
        [Fact]
        public async Task ProcessPaymentAsync_ValidOrder_MarksPaymentAsPaid()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalAmount = 55000,
                Status = "CheckedOut"
            };

            var fakeOrderRepository = new FakeOrderRepository
            {
                ExistingOrder = order
            };

            var fakePaymentRepository = new FakePaymentRepository();

            var service = new PaymentService(
                fakePaymentRepository,
                fakeOrderRepository);

            var payment = new Payment
            {
                OrderId = 1,
                PaymentMethod = "Card",
                Status = "Pending"
            };

            // Act
            var result = await service.ProcessPaymentAsync(payment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(55000, result.Amount);
            Assert.Equal("Paid", result.Status);
            Assert.Equal("Paid", order.Status);
        }
    }

    public class FakeProductRepository : IProductRepository
    {
        public Product? ExistingProduct { get; set; }
        public Task<Product> AddAsync(Product product)
        {
            return Task.FromResult(product);
        }

        public Task<IEnumerable<Product>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<Product>>(
                new List<Product>());
        }

        public Task<Product?> GetByIdAsync(int id)
        {
            return Task.FromResult(ExistingProduct);
        }

        public Task<Product?> UpdateAsync(Product product)
        {
            return Task.FromResult<Product?>(product);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return Task.FromResult(true);
        }
    }
    public class FakeOrderRepository : IOrderRepository
    {
        public Order? ExistingOrder { get; set; }

        public Task<Order> CreateAsync(Order order)
        {
            return Task.FromResult(order);
        }

        public Task<Order?> GetByIdAsync(int id)
        {
            return Task.FromResult(ExistingOrder);
        }

        public Task<List<Order>> GetByUserIdAsync(int userId)
        {
            return Task.FromResult(new List<Order>());
        }

        public Task UpdateAsync(Order order)
        {
            return Task.CompletedTask;
        }
    }
    public class FakePaymentRepository : IPaymentRepository
    {
        public Task<Payment> CreateAsync(Payment payment)
        {
            return Task.FromResult(payment);
        }

        public Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return Task.FromResult<Payment?>(null);
        }
    }
}