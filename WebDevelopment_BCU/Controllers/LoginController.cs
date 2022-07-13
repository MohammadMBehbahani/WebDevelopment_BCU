using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Models.Dto;
using WebDevelopment_BCU.Models.ViewData;

namespace WebDevelopment_BCU.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly DataBaseContext _context;

        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager, DataBaseContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var finalData = new HomeData
            {
                About = _context.About.FirstOrDefault()
            };

            return View(finalData);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Login Input)
        {
            if (string.IsNullOrEmpty(Input.Password) || string.IsNullOrEmpty(Input.UserName))
            {
                TempData["ErrorLogin"] = "UserName or Password is empty";
                return View();
            }
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(Input.UserName);
                    var roles = await _userManager.GetRolesAsync(user);
                    
                    if (roles.Contains("admin"))
                    {
                        return Redirect("/Admin/Home/Index");
                    }
                    else
                    {
                            return Redirect("/Home/Index");
                    }
                }
                else
                {
                    TempData["ErrorLogin"] = "UserName or Password Incorrect";
                }
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Index");
        }

		[HttpPost]
		public async Task<ActionResult> CreateAccount(Register Input)
		{
			if (string.IsNullOrEmpty(Input.Email) || string.IsNullOrEmpty(Input.ConfirmPassword) || string.IsNullOrEmpty(Input.Password))
			{
				TempData["ErrorRegister"] = "Please fill the required field";
				return RedirectToAction(nameof(Index));
			}
			
			if (!Input.Password.Equals(Input.ConfirmPassword))
			{
				TempData["ErrorRegister"] = "Password does not match";
				return RedirectToAction(nameof(Index));

			}

            if (_context.Users.Any(p=>p.UserName == Input.Email))
            {
                TempData["ErrorRegister"] = "UserName already exist";
                return RedirectToAction(nameof(Index));
            }

			var userItem = new User
			{
				UserName = Input.Email.ToString(),
				Email = Input.Email
			};
			var result = await _userManager.CreateAsync(userItem, Input.Password);
			if (result.Succeeded)
			{
                await _userManager.AddToRoleAsync(userItem, "normaluser");
            }

			await _signInManager.SignInAsync(userItem, isPersistent: false);
            return Redirect("/Home/Index");
        }
	}
}
