using BusinessEntities;
using System;

namespace WebApi.Models.Orders
{
    public class OrderData
    {
        public OrderData(Order order)
        {
            OrderId = order.OrderId;
            ProductId = order.ProductId;
            Quantity = order.Quantity;
            OrderDate = order.OrderDate;
        }

        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}