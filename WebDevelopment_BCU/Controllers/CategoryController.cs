using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataBaseContext _context;

        public CategoryController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(RequestGetList dto)
        {
            var data = _context.Category.AsQueryable();
            if (!string.IsNullOrWhiteSpace(dto.SearchKey))
            {
                data = data.Where(p =>  p.Name.Contains(dto.SearchKey)
                                        || p.InserDate.Date.Equals(dto.Date.Value.Date)

                                        || p.Id.ToString().Equals(dto.SearchKey)).OrderByDescending(p => p.Id);

            }
            var dataList = data.ToPages(dto.Page ?? 1, dto.PageSize ?? 10, out int rowsCount).ToList();
            return View(dataList);
        }
    }
}
