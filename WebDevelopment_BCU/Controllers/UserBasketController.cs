using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDevelopment_BCU.Models;
using WebDevelopment_BCU.Models.ViewData;

namespace WebDevelopment_BCU.Controllers
{
    public class UserBasketController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserBasketController(DataBaseContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            var finalData = new HomeData
            {
                About = _context.About.FirstOrDefault()
            };

            return View(finalData);
        }

        [HttpGet]
        public async Task<IActionResult> GetDataBasket()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var UserBasket = _context.UserBasket.Include(p => p.Items).FirstOrDefault(p => p.UserId == user.Id);
                List<ShoppingCartItem> Baskets = new();
                ShoppingCartItem basket = null;

                if (UserBasket != null)
                {
                    foreach (var item in UserBasket.Items)
                    {
                        basket = new ShoppingCartItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UserBasketId = item.UserBasketId
                        };

                        Baskets.Add(basket);
                    }
                }
                decimal total = 0;
                foreach (var item in Baskets)
                {
                    total += _context.Product.Find(item.ProductId).Price * item.Quantity;
                }

                var dataBasket = Baskets.Select(p => new
                {
                    p.Id,
                    Name = _context.Product.Find(p.ProductId).Name,
                    Price = _context.Product.Find(p.ProductId).Price,
                    p.ProductId,
                    Count = p.Quantity,
                    TotalCount = _context.UserBasket.Find(p.UserBasketId).Items.Sum(p => p.Quantity),
                    TotalPrice = total
                }).ToList();

                return Ok(dataBasket);

            }
            else
            {
                string key = "UserBasket";
                var _Basket = Request.Cookies.Where(p => p.Key.Contains(key)).ToList();
                List<ShoppingCartItem> Baskets = new();
                ShoppingCartItem basket = null;

                foreach (var item in _Basket)
                {
                    basket = new ShoppingCartItem
                    {
                        ProductId = Convert.ToInt64(item.Value.Split("_")[0]),
                        Quantity = Convert.ToInt32(item.Value.Split("_")[1])
                    };

                    Baskets.Add(basket);
                }

                decimal total = 0;
                foreach (var item in Baskets)
                {
                    total += _context.Product.Find(item.ProductId).Price * item.Quantity;
                }

                var dataBasket = Baskets.Select(p => new
                {
                    Name = _context.Product.Find(p.ProductId).Name,
                    Price = _context.Product.Find(p.ProductId).Price,
                    p.ProductId,
                    Count = p.Quantity,
                    TotalCount = Baskets.Sum(p => p.Quantity),
                    TotalPrice = total,
                }).ToList();

                return Ok(dataBasket);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(ShoppingCartItem basket)
        {
            if (basket.Quantity <= 0)
            {
                return BadRequest("Count cannot be 0 or less");
            }
            DateTimeOffset Expires = DateTime.Now.AddDays(1);
            Product product = await _context.Product.FindAsync(basket.ProductId);
            basket.ProductId = product.Id;

            string Key = "UserBasket" + "_" + basket.ProductId;

            var ProductName = _context.Product.Find(basket.ProductId).Name;
            var ProductPrice = _context.Product.Find(basket.ProductId).Price;
            var ProductImages = _context.Product.Find(basket.ProductId).ProductImages == null ? "" : _context.Product.Find(basket.ProductId).ProductImages.FirstOrDefault().Src;

            string Value = basket.ProductId + "_" + basket.Quantity + "_" + ProductName + "_" + ProductPrice + "_" + ProductImages;
            //ifuser online add to basket sql

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                bool existbasket = await _context.UserBasket.AnyAsync(p => p.UserId == user.Id);

                if (existbasket)
                {
                    if (_context.ShoppingCartItem.Any(p => p.ProductId == basket.ProductId))
                    {
                        var PP = _context.ShoppingCartItem.FirstOrDefault(p => p.ProductId == basket.ProductId);
                        PP.Quantity = basket.Quantity;
                    }
                    else
                    {
                        await _context.ShoppingCartItem.AddAsync(new ShoppingCartItem
                        {
                            ProductId = basket.ProductId,
                            Quantity = basket.Quantity,
                            UserBasketId = _context.UserBasket.FirstOrDefault(p => p.UserId == user.Id).Id
                        });
                    }
                }

                else
                {
                    await _context.UserBasket.AddAsync(new UserBasket
                    {
                        UserId = user.Id,
                        Items = new System.Collections.Generic.List<ShoppingCartItem>
                    {
                        new ShoppingCartItem
                        {
                            ProductId = basket.ProductId,
                            Quantity = basket.Quantity,
                        },
                    }
                    }); ;
                }
                await _context.SaveChangesAsync();
            }



            if (Get(Key) != null)
            {
                Remove(Key);
                Set(Key, Value, Expires);

            }
            else
            {
                Set(Key, Value, Expires);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItemBasket(ShoppingCartItem basket)
        {
            string Key = "UserBasket" + "_" + basket.ProductId;

            ////ifuser online remove from basket sql
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userbasket = _context.UserBasket
                    .FirstOrDefault(p => p.UserId == user.Id
                                    && p.Items.Any(w => w.ProductId == basket.ProductId));
                if (userbasket == null)
                {
                    return BadRequest("Empty Basket");
                }

                _context.UserBasket.Remove(userbasket);
                await _context.SaveChangesAsync();
                return Ok();
            }



            if (Get(Key) != null)
            {
                Remove(Key);
            }

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> ReduceItemBasket(ShoppingCartItem basket)
        {
            string Key = "UserBasket" + "_" + basket.ProductId;

            ////ifuser online remove from basket sql
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userbasket = _context.UserBasket
                    .FirstOrDefault(p => p.UserId == user.Id
                                    && p.Items.Any(w => w.ProductId == basket.ProductId));
                if (userbasket == null)
                {
                    return BadRequest("Empty Basket");
                }
                var data = _context.ShoppingCartItem.FirstOrDefault(p => p.ProductId == basket.ProductId);
                if (data.Quantity - 1 <= 0)
                {
                    await RemoveItemBasket(basket);
                }
                else
                {
                    data.Quantity -= 1;
                }
                await _context.SaveChangesAsync();
                return Ok();
            }


            if (basket.Quantity - 1 <= 0)
            {
                Remove(Key);
            }
            else if (Get(Key) != null)
            {
                DateTimeOffset Expires = DateTime.Now.AddDays(1);

                var ProductName = _context.Product.Find(basket.ProductId).Name;
                var ProductPrice = _context.Product.Find(basket.ProductId).Price;
                var ProductImages = _context.Product.Find(basket.ProductId).ProductImages == null ? "" : _context.Product.Find(basket.ProductId).ProductImages.FirstOrDefault().Src;

                basket.Quantity -= 1;

                string Value = basket.ProductId + "_" + basket.Quantity + "_" + ProductName + "_" + ProductPrice + "_" + ProductImages;

                Reduce(Key, Value, Expires);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAllBasket()
        {
            string Key = "UserBasket";

            ////ifuser online add to basket sql

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userbasket = _context.UserBasket
                    .FirstOrDefault(p => p.UserId == user.Id);

                if (userbasket == null)
                {
                    return BadRequest("Empty Basket");
                }

                _context.UserBasket.Remove(userbasket);
                await _context.SaveChangesAsync();
                return Ok();
            }


            RemoveAll(Key);
            return Ok();

        }

        #region coocki
        public void Set(string key, string value, DateTimeOffset expireTime)
        {
            CookieOptions option = new()
            {
                Expires = expireTime
            };

            Response.Cookies.Append(key, value, option);
        }
        public string Get(string key)
        {
            try
            {
                return Request.Cookies[key];

            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public void Remove(string key)
        {
            Response.Cookies.Delete(key);
        }

        public void Reduce(string key, string value, DateTimeOffset expireTime)
        {
            Remove(key);
            Set(key, value, expireTime);
        }
        public void RemoveAll(string key)
        {
            var baskets = Request.Cookies.Where(p => p.Key.Contains(key)).ToList();
            foreach (var item in baskets)
            {
                Response.Cookies.Delete(item.Key);
            }
        }
        public void Clear()
        {
            string key = "UserBasket";
            var _Basket = Request.Cookies.Where(p => p.Key.Contains(key)).ToList();

            foreach (var item in _Basket)
            {
                Response.Cookies.Delete(item.Key);
            }
        }
        #endregion
    }
}
