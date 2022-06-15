using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class ProductImage:ModelBase
    {
        public string Name { get; set; }
        public string Src { get; set; }
        public long ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
