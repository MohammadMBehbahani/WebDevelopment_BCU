using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class Categories:ModelBase
    {
        public string Name { get; set; }
        public long ParentId { get; set; } = 0;
        public string Image { get; set; }
    }
}
