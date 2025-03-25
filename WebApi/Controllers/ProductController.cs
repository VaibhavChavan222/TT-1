using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : ApiController
    {
        private static readonly List<Product> Products = new List<Product>();

        [Route("{productId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateProduct(Guid productId, [FromBody] ProductModel model)
        {
            var product = new Product
            {
                ProductId = productId,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            };
            Products.Add(product);
            return Request.CreateResponse(HttpStatusCode.OK, new ProductData(product));
        }

        [Route("{productId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateProduct(Guid productId, [FromBody] ProductModel model)
        {
            var product = Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product not found.");
            }
            product.Name = model.Name;
            product.Price = model.Price;
            product.Stock = model.Stock;
            return Request.CreateResponse(HttpStatusCode.OK, new ProductData(product));
        }

        [Route("{productId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteProduct(Guid productId)
        {
            var product = Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product not found.");
            }
            Products.Remove(product);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("{productId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetProduct(Guid productId)
        {
            var product = Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product not found.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, new ProductData(product));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetProducts(string name = null, decimal? price = null, int? stock = null)
        {
            var filteredProducts = Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                filteredProducts = filteredProducts.Where(p => p.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (price.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Price == price.Value);
            }

            if (stock.HasValue)
            {
                filteredProducts = filteredProducts.Where(p => p.Stock == stock.Value);
            }

            var productData = filteredProducts.Select(p => new ProductData(p)).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, productData);
        }
    }
}
