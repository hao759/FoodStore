using CuaHangDoAn.Data;
using CuaHangDoAn.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CuaHangDoAn.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        CuaHangDoAnContext _context;

        public HomeController(ILogger<HomeController> logger,CuaHangDoAnContext context)
        {
            _logger = logger;
            _context = context;
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




    }
}