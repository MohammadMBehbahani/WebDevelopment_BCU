using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _environment;

        public NewsController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var data = _context.News.OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(News dto, IFormFile file)
        {
            var resultUpload = new UploadFile(_environment).UploadFileFunction(file, @"News\");
            dto.Image = resultUpload.FileNameAddress;

            if (dto.Id == 0)
            {
                //add
                await _context.News.AddAsync(dto);
            }
            else
            {
                //update
                var prdata = await _context.News.FindAsync(dto.Id);
                if (string.IsNullOrEmpty(dto.Image))
                {
                    dto.Image = prdata.Image;
                }
                else
                {
                    prdata.Image = dto.Image;

                }
                dto.InserDate = prdata.InserDate;
                prdata.Title = dto.Title;
                prdata.Caption = dto.Caption;
                prdata.Text = dto.Text;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(long id)
        {
            _context.News.Remove(await _context.News.FindAsync(id));
            await _context.SaveChangesAsync();
        }
    }
}
