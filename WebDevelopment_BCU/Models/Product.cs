using System.Collections.Generic;
using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class Product:ModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<UserOrderDetails> UserOrderDetails { get; set; }
    }
}
