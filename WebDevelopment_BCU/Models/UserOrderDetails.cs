using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class UserOrderDetails : ModelBase
    {
        public long UserOrdersId { get; set; }
        public virtual UserOrders UserOrders { get; set; }

        public long ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
