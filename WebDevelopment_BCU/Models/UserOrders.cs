using System;
using System.Collections.Generic;
using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class UserOrders : ModelBase
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string UserOrderCode { get; set; } = new Random().Next().ToString();
        public DateTime DateRequest { get; set; }
        public DateTime? DateDelivery { get; set; }
        public int Status{ get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<UserOrderDetails> UserOrderDetails { get; set; }

    }
}
