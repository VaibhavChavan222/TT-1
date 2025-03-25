using System;
using BusinessEntities;

namespace WebApi.Models.Products
{
    public class ProductData
    {
        public ProductData(Product product)
        {
            ProductId = product.ProductId;
            Name = product.Name;
            Price = product.Price;
            Stock = product.Stock;
        }

        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}