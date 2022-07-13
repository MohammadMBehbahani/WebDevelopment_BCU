using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebDevelopment_BCU.Models;

namespace WebDevelopment_BCU.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly DataBaseContext _context;

        public UserController(DataBaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Users.OrderByDescending(p => p.Id).ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Post(User dto)
        {

            if (string.IsNullOrEmpty(dto.Id))
            {
                //add
                await _context.Users.AddAsync(dto);
            }
            else
            {
                //update
                var prdata = await _context.Users.FindAsync(dto.Id);
                prdata.FullName = dto.FullName;
                prdata.UserName = dto.Email;
                prdata.Email = dto.Email;
                prdata.Gender = dto.Gender;
                prdata.Age = dto.Age;
            }
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(long id)
        {
            _context.Users.Remove(await _context.Users.FindAsync(id));
            await _context.SaveChangesAsync();
        }
    }
}
