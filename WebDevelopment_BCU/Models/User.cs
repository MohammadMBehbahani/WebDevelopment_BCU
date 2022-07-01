using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WebDevelopment_BCU.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public bool? Gender { get; set; }
        public bool IsDefault { get; set; } = false;
        public int? Age { get; set; }
        public ICollection<UserOrders> UserOrders { get; set; }
        public ICollection<UserBasket> UserBasket { get; set; }

    }
}
