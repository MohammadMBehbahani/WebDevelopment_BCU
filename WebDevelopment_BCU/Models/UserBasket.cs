using System.Collections.Generic;
using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class UserBasket : ModelBase
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public int Quantity { get; set; }

        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += item.Product.Price * item.Quantity;
                }
                return totalprice;
            }
        }
    }

    
}
