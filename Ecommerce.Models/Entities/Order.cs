using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CraetedAt { get; set; } = DateTime.UtcNow;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
