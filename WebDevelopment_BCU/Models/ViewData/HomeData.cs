using System.Collections.Generic;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Models.ViewData
{
    public class HomeData
    {
        public About About { get; set; }
        public ResultPagination<Slider> Slider { get; set; }
        public ResultPagination<Category> Category { get; set; }
        public ResultPagination<Product> Products { get; set; }
        public ResultPagination<UserOrders> UserOrders { get; set; }
        public ResultPagination<UserOrderDetails> UserOrderDetails { get; set; }
        public Product Product { get; set; }
        public User User { get; set; }
        public ResultPagination<News> News { get; set; }
        public News NewsDetail { get; set; }
    }
}
