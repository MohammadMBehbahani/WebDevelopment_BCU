using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Models.ViewData;

namespace WebDevelopment_BCU.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly UserManager<User> _userManager;

        public UserProfileController(DataBaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var about = _context.About.FirstOrDefault();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var data = new HomeData
            {
                About = about,
                User = user
            };


            return View(data);
        }
    }
}
