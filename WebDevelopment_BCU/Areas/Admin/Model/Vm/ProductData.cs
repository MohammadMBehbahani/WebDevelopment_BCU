using System.Collections.Generic;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Areas.Admin.Model.Vm
{
    public class ProductData
    {
        public List<Category> CategoryList { get; set; }
        public ResultPagination<Product> ProductList { get; set; }

    }
}
