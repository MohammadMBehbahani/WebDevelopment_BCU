using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Models.ViewData;
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
            var data = _context.Product.OrderByDescending(p => p.Id).Include(p => p.ProductImages).AsQueryable();
            var TotalCount = data.Count();

            if (!string.IsNullOrWhiteSpace(dto.SearchKey))
            {
                data = data.Where(p => p.Description.Contains(dto.SearchKey)
                                        || p.Name.Contains(dto.SearchKey)
                                        || p.Price == Convert.ToInt64(dto.SearchKey)
                                        || p.CategoryId == Convert.ToInt64(dto.SearchKey)

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

            var finalData = new HomeData
            {
                About = _context.About.FirstOrDefault(),
                Products = datafinal
            };

            return View(finalData);
        }

        public IActionResult Details(RequestGetList dto)
        {

            if (!string.IsNullOrWhiteSpace(dto.SearchKey))
            {
                if (_context.Product.FirstOrDefault(p => p.Id == Convert.ToInt64(dto.SearchKey)) == null)
                {
                    return RedirectToAction("Index");
                }
                var finalData = new HomeData
                {
                    About = _context.About.FirstOrDefault(),
                    Product = _context.Product.Include(p => p.ProductImages).FirstOrDefault(p=> p.Id == Convert.ToInt64(dto.SearchKey))
                };

                return View(finalData);
            }

            return RedirectToAction("Index");
        }
    }
}
