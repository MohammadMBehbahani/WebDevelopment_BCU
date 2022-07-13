using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class ShoppingCartItem : ModelBase
    {
        public int Quantity { get; set; }
        public long ProductId { get; set; }
        public virtual Product Product { get; set; }
        public long UserBasketId { get; set; }
    }
}
