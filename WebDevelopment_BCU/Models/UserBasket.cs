using System.Collections.Generic;
using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class UserBasket : ModelBase
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

    }

    
}
