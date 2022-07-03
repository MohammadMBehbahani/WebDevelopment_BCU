using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataBaseContext _context;

        public ProductController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(RequestGetList dto)
        {
            var data = _context.Product.OrderByDescending(p => p.Id).AsQueryable();
            var TotalCount = data.Count();

            if (!string.IsNullOrWhiteSpace(dto.SearchKey))
            {
                data = data.Where(p => p.Description.Contains(dto.SearchKey)
                                        || p.Name.Contains(dto.SearchKey)
                                        || p.Price.Equals(dto.SearchKey)

                                        || p.Id.ToString().Equals(dto.SearchKey)).OrderByDescending(p => p.Id);

            }
            var dataList = data.ToPages(dto.Page ?? 1, dto.PageSize ?? 10, out int rowsCount).ToList();
            var pagesize = dto.PageSize ?? 10;
            decimal NumberOfPage = Math.Ceiling(Convert.ToDecimal(rowsCount / pagesize)) + 1;

            var datafinal = new ResultPagination<Product>
            {
                CurrentPage = dto.Page ?? 1,
                NumberOfPage = NumberOfPage,
                PageSize = dto.PageSize ?? 10,
                Rows = rowsCount,
                TotalCount = TotalCount,
                ListData = dataList
            };

            return View(datafinal);
        }

        
    }
}
