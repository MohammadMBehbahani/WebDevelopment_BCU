using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Models.ViewData;
using WebDevelopment_BCU.Utility;

namespace WebDevelopment_BCU.Controllers
{
    [Authorize]

    public class UserOrderController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly UserManager<User> _userManager;

        public UserOrderController(DataBaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(RequestGetList dto)
        {
            var about = _context.About.FirstOrDefault();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var data = new HomeData
            {
                About = about,
                UserOrders = GetDataUserOrders(user, dto)
            };


            return View(data);
        }


        public IActionResult Details(RequestGetList dto, long UserOrderId)
        {
            var about = _context.About.FirstOrDefault();

            var data = new HomeData
            {
                About = about,
                UserOrderDetails = GetDataUserOrdersDetails(dto, UserOrderId)
            };


            return View(data);
        }


        private ResultPagination<UserOrders> GetDataUserOrders(User user, RequestGetList dto)
        {
            var dataList = _context.UserOrders.Where(p => p.UserId == user.Id).ToPages(dto.Page ?? 1, dto.PageSize ?? 10, out int rowsCount).ToList();
            var TotalCount = _context.UserOrders.Count();
            var pagesize = dto.PageSize ?? 10;


            decimal NumberOfPage = Math.Ceiling(Convert.ToDecimal(rowsCount / pagesize)) + 1;

            var datafinal = new ResultPagination<UserOrders>
            {
                CurrentPage = dto.Page ?? 1,
                NumberOfPage = NumberOfPage,
                PageSize = dto.PageSize ?? 10,
                Rows = rowsCount,
                TotalCount = TotalCount,
                ListData = dataList
            };
            return datafinal;
        }

        private ResultPagination<UserOrderDetails> GetDataUserOrdersDetails(RequestGetList dto, long UserOrderId)
        {
            var dataList = _context.UserOrderDetails
                    .Include(p => p.Product)
                    .Where(p => p.UserOrdersId == UserOrderId)
                    .ToPages(dto.Page ?? 1, dto.PageSize ?? 10, out int rowsCount).ToList();

            var TotalCount = _context.UserOrderDetails.Count();
            var pagesize = dto.PageSize ?? 10;


            decimal NumberOfPage = Math.Ceiling(Convert.ToDecimal(rowsCount / pagesize)) + 1;

            var datafinal = new ResultPagination<UserOrderDetails>
            {
                CurrentPage = dto.Page ?? 1,
                NumberOfPage = NumberOfPage,
                PageSize = dto.PageSize ?? 10,
                Rows = rowsCount,
                TotalCount = TotalCount,
                ListData = dataList
            };
            return datafinal;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var UserBasket = _context.UserBasket.Include(p => p.Items).FirstOrDefault(p => p.UserId == user.Id);


            var order = new UserOrders
            {
                UserId = user.Id,
                DateDelivery = DateTime.Now,
                DateRequest = DateTime.Now,
                Status = 0,
            };

            await _context.UserOrders.AddAsync(order);

            await _context.SaveChangesAsync();

            if (UserBasket != null)
            {
                foreach (var item in UserBasket.Items)
                {
                    var userOrderDetail = new UserOrderDetails
                    {
                        Quantity = item.Quantity,
                        ProductId = item.ProductId,
                        UserOrdersId = order.Id
                    };
                    await _context.UserOrderDetails.AddAsync(userOrderDetail);

                    await _context.SaveChangesAsync();

                }
                _context.ShoppingCartItem.RemoveRange(_context.ShoppingCartItem.Where(p => p.UserBasketId == UserBasket.Id));
                await _context.SaveChangesAsync();

                _context.UserBasket.RemoveRange(_context.UserBasket.Where(p => p.UserId == user.Id));
                await _context.SaveChangesAsync();

            }


            var UserOrderDetails = _context.UserOrderDetails.Include(p => p.Product).Where(p => p.UserOrdersId == order.Id);

            decimal totalprice = 0;
            foreach (var item in UserOrderDetails)
            {
                totalprice += item.Product.Price * item.Quantity;
            }

            order.TotalPrice = totalprice;

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));

        }
    }
}
