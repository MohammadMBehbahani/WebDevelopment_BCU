using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Models.ViewData;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseContext _context;

        public HomeController(ILogger<HomeController> logger, DataBaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var about = _context.About.FirstOrDefault();
            var Slider = GetDataSlider();
            var Category = GetDataCategory();
            var Product = GetDataProduct();
            var News = GetDataNews();


            var data = new HomeData
            {
                Slider = Slider,
                About = about,
                Category = Category,
                News = News,
                Products = Product
            };


            return View(data);
        }
        private ResultPagination<Slider> GetDataSlider()
        {
            var dataList = _context.Slider.ToPages(1, 10, out int rowsCount).ToList();
            var TotalCount = _context.Slider.Count();
            var pagesize = 10;


            decimal NumberOfPage = Math.Ceiling(Convert.ToDecimal(rowsCount / pagesize)) + 1;

            var datafinal = new ResultPagination<Slider>
            {
                CurrentPage = 1,
                NumberOfPage = NumberOfPage,
                PageSize = 10,
                Rows = rowsCount,
                TotalCount = TotalCount,
                ListData = dataList
            };
            return datafinal;
        }
        private ResultPagination<Category> GetDataCategory()
        {
            var dataList = _context.Category.ToPages(1, 10, out int rowsCount).ToList();
            var TotalCount = _context.Category.Count();
            var pagesize = 10;


            decimal NumberOfPage = Math.Ceiling(Convert.ToDecimal(rowsCount / pagesize)) + 1;

            var datafinal = new ResultPagination<Category>
            {
                CurrentPage = 1,
                NumberOfPage = NumberOfPage,
                PageSize = 10,
                Rows = rowsCount,
                TotalCount = TotalCount,
                ListData = dataList
            };
            return datafinal;
        }

        private ResultPagination<News> GetDataNews()
        {
            var dataList = _context.News.ToPages(1, 10, out int rowsCount).ToList();
            var TotalCount = _context.News.Count();
            var pagesize = 10;


            decimal NumberOfPage = Math.Ceiling(Convert.ToDecimal(rowsCount / pagesize)) + 1;

            var datafinal = new ResultPagination<News>
            {
                CurrentPage = 1,
                NumberOfPage = NumberOfPage,
                PageSize = 10,
                Rows = rowsCount,
                TotalCount = TotalCount,
                ListData = dataList
            };
            return datafinal;
        }

        private ResultPagination<Product> GetDataProduct()
        {
            var dataList = _context.Product.Include(p=>p.ProductImages).ToPages(1, 10, out int rowsCount).ToList();
            var TotalCount = _context.Product.Count();
            var pagesize = 10;


            decimal NumberOfPage = Math.Ceiling(Convert.ToDecimal(rowsCount / pagesize)) + 1;

            var datafinal = new ResultPagination<Product>
            {
                CurrentPage = 1,
                NumberOfPage = NumberOfPage,
                PageSize = 10,
                Rows = rowsCount,
                TotalCount = TotalCount,
                ListData = dataList
            };
            return datafinal;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
