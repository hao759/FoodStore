using AspNetCoreHero.ToastNotification.Abstractions;
using CuaHangDoAn.Data;
using CuaHangDoAn.Models;
using CuaHangDoAn.ModelView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CuaHangDoAn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        CuaHangDoAnContext _context;
        public INotyfService _notifyService { get; }

        public Cart Cart { get; set; }
        public HomeController(ILogger<HomeController> logger,CuaHangDoAnContext context, INotyfService notifyService)
        {
            _logger = logger;
            _context = context;
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
            var listProduct= _context.Products.AsNoTracking().ToList();
            return View(listProduct);
        }
        public IActionResult DetailProduct(int IDProduct)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(s=>s.Id==IDProduct);
            ViewBag.RelateProducts=_context.Products.AsNoTracking().Where(s=>s.CateID== product.CateID && s.Id != IDProduct).ToList();
            return View(product);
        }

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



    }
}