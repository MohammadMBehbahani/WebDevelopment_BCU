using Microsoft.AspNetCore.Mvc;

namespace WebDevelopment_BCU.Controllers
{
    public class ProductController: Controller
    {
        public IActionResult Index()
        {

            //fetch data from database
            return View();
        }
    }
}
