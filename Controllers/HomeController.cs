using AspNetCoreHero.ToastNotification.Abstractions;
using CuaHangDoAn.Data;
using CuaHangDoAn.Models;
using CuaHangDoAn.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace CuaHangDoAn.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public UserManager<AppUser> userManager { get; set; }
        CuaHangDoAnContext _context;
        public INotyfService _notifyService { get; }

        public Cart Cart { get; set; }
        public HomeController(ILogger<HomeController> logger,CuaHangDoAnContext context, INotyfService notifyService, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _context = context;
            _notifyService = notifyService;
            this.userManager = userManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var listProduct= _context.Products.AsNoTracking().ToList();
            return View(listProduct);
        }
        [AllowAnonymous]
        public IActionResult DetailProduct(int IDProduct)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(s=>s.Id==IDProduct);
            ViewBag.RelateProducts=_context.Products.AsNoTracking().Where(s=>s.CateID== product.CateID && s.Id != IDProduct).ToList();
            return View(product);
        }
        [AllowAnonymous]
        public IActionResult Category(int IDCategory)
        {
            var listProduct = _context.Products.AsNoTracking().Where(s => s.CateID == IDCategory).ToList();
            return View(listProduct);
        }

        public Cart GetCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (sessionCart != null)
                 Cart = JsonConvert.DeserializeObject<Cart>(sessionCart);
            else 
            {
                Cart = new Cart();
                Cart.Items = new List<CartItem>();
            }
            return Cart;
        }

        //Api
        public IActionResult AddProductToCart(int idProduct)
        {
            var product= _context.Products.AsNoTracking().FirstOrDefault(s=>s.Id==idProduct);
            Cart cart=GetCart();
            var item = cart.Items.FirstOrDefault(s => s.product.Id == idProduct);
            if(item!=null )
                item.quantity += 1;
            else
            {
                var itemCart = new CartItem()
                {
                    quantity = 1,
                    product = product
                };
                cart.Items.Add(itemCart);
            }
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            //var a = JsonConvert.DeserializeObject<Cart>(HttpContext.Session.GetString("Cart"));
            //_notifyService.Success("Thanh Cong");
            return Json(new
            {
                status = "success"
            });
        }


        public IActionResult Shoping_Cart()
        {
            Cart cart = GetCart();
            ViewBag.TotalMoney = cart.Items.Sum(s => s.product.Price*s.quantity);
            return View(cart);
        }

        [HttpPost]
        public IActionResult DeleteCartItem(int idProduct)
        {
            Cart cart = GetCart();
            try
            {
                cart.Items.RemoveAll(s=>s.product.Id==idProduct);
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                RedirectToAction("Index");
            }
            //return RedirectToAction("Shoping_Cart");
            return Json(new
            {
                status = "success"
            });
        }

        [HttpPost]
        public IActionResult UpdateCartItem(int[] quantity,int[] IdProduct)
        {
            Cart cart = GetCart();
            for(int i=0; i < quantity.Length; i++)
            {
                if (quantity[i]==0)
                    cart.Items.RemoveAll(s => s.product.Id == IdProduct[i]);
                else
                    cart.Items[i].quantity = quantity[i];
            }
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Shoping_Cart");
        }

        public IActionResult Checkout()
        {
           
            return View();
        }
        public  async Task<IActionResult> MyProfile()
        {
            //var user = User.Identity.GetUserId();
            var user= await userManager.GetUserAsync(HttpContext.User);
            var profileUser = new InfoModelView
            {
                UserName = user.UserName,
                ID = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View(profileUser);
        }


        public async Task<IActionResult> ConfirmOrderAsync()
        {
            Cart cart = GetCart();
            var newOrder = new Order();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            newOrder.UserID = user.Id;
            newOrder.TotalPrice= (decimal)cart.Items.Sum(s => s.product.Price * s.quantity);
            _context.Add(newOrder);
            _context.SaveChanges();

            foreach (var item in cart.Items)
            {
                var newOrderDetail = new OrderDetails()
                {
                    OrderID = newOrder.OrderId,
                    ProductID = item.product.Id,
                    quantity = item.quantity,
                };
                _context.Add(newOrderDetail);
            }
            _context.SaveChanges();
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");   
        }

    }
}