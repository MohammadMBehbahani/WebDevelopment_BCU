using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebDevelopment_BCU.Models;

namespace WebDevelopment_BCU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly DataBaseContext _context;

        public AboutController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.About.OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(About dto)
        {
            if (dto.Id == 0)
            {
                //add
                await _context.About.AddAsync(dto);
            }
            else
            {
                //update
                var prdata = await _context.News.FindAsync(dto.Id);
               
                dto.InserDate = prdata.InserDate;
                _context.About.Update(dto);
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(long id)
        {
            _context.About.Remove(await _context.About.FindAsync(id));
            await _context.SaveChangesAsync();
        }
    }
}
