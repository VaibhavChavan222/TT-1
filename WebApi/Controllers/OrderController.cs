using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using WebApi.Models.Orders;

namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    public class OrderController : ApiController
    {
        private static readonly List<Order> Orders = new List<Order>();

        [Route("{orderId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateOrder(Guid orderId, [FromBody] OrderModel model)
        {
            var order = new Order
            {
                OrderId = orderId,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
                OrderDate = model.OrderDate
            };
            Orders.Add(order);
            return Request.CreateResponse(HttpStatusCode.OK, new OrderData(order));
        }

        [Route("{orderId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateOrder(Guid orderId, [FromBody] OrderModel model)
        {
            var order = Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found.");
            }
            order.ProductId = model.ProductId;
            order.Quantity = model.Quantity;
            order.OrderDate = model.OrderDate;
            return Request.CreateResponse(HttpStatusCode.OK, new OrderData(order));
        }

        [Route("{orderId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrder(Guid orderId)
        {
            var order = Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found.");
            }
            Orders.Remove(order);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{orderId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetOrder(Guid orderId)
        {
            var order = Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Order not found.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new OrderData(order));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetOrders(Guid? productId = null, DateTime? orderDate = null, int? quantity = null)
        {
            var filteredOrders = Orders.AsQueryable();

            if (productId.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.ProductId == productId.Value);
            }

            if (orderDate.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.OrderDate.Date == orderDate.Value.Date);
            }

            if (quantity.HasValue)
            {
                filteredOrders = filteredOrders.Where(o => o.Quantity == quantity.Value);
            }

            var orderData = filteredOrders.Select(o => new OrderData(o)).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, orderData);
        }
    }
}
