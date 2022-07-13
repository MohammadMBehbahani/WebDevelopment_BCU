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
    public class SliderController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _environment;

        public SliderController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var data = _context.Slider.OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Slider dto, IFormFile file)
        {
            var resultUpload = new UploadFile(_environment).UploadFileFunction(file, @"Slider\");
            dto.Image = resultUpload.FileNameAddress;

            if (dto.Id == 0)
            {
                //add
                await _context.Slider.AddAsync(dto);
            }
            else
            {
                //update
                var prdata = await _context.Slider.FindAsync(dto.Id);
                if (string.IsNullOrEmpty(dto.Image))
                {
                    dto.Image = prdata.Image;
                }
                else
                {
                    prdata.Image = dto.Image;

                }
                dto.InserDate = prdata.InserDate;
                prdata.Price = dto.Price;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(long id)
        {
            _context.Slider.Remove(await _context.Slider.FindAsync(id));
            await _context.SaveChangesAsync();
        }
    }
}
