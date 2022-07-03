using WebDevelopment_BCU.Models.Common;

namespace WebDevelopment_BCU.Models
{
    public class News : ModelBase
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public string Text { get; set; }
    }
}
