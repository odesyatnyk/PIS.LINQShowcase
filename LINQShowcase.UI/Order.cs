using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQShowcase.UI
{
    public class Order
    {
        public Order()
        {
            OrderedProducts = new List<Product>();
        }

        public int IdOrder { get; set; }
        public string Client { get; set; }
        public List<Product> OrderedProducts{ get; set; }

        public Order AddProduct(Product product)
        {
            if (OrderedProducts.Any(x => x.IdProduct == product.IdProduct))
                throw new Exception("Product already in order");

            OrderedProducts.Add(product);
            return this;
        }

        public decimal TotalPrice => OrderedProducts.Sum(x => x.Price);
    }
}
