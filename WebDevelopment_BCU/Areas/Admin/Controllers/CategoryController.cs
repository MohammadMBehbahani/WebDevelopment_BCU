using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(DataBaseContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var data = _context.Category.OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Category dto, IFormFile file)
        {
            var resultUpload = new UploadFile(_environment).UploadFileFunction(file, @"Category\");
            dto.Image = resultUpload.FileNameAddress;

            if (dto.Id == 0)
            {
                //add
                await _context.Category.AddAsync(dto);
            }
            else
            {
                //update
                var prdata = await _context.Category.FindAsync(dto.Id);
                if (string.IsNullOrEmpty(dto.Image))
                {
                    dto.Image = prdata.Image;
                }
                else
                {
                    prdata.Image = dto.Image;

                }
                dto.InserDate = prdata.InserDate;
                prdata.Name = dto.Name;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(long id)
        {
            _context.Category.Remove(await _context.Category.FindAsync(id));
            await _context.SaveChangesAsync();
        }
    }
}
