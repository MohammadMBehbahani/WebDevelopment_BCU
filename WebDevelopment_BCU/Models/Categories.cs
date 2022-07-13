using System.Collections.Generic;
using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class Category:ModelBase
    {
        public string Name { get; set; }
        public long ParentId { get; set; } = 0;
        public string Image { get; set; }
        public ICollection<Product> Product { get; set; }

    }
}
