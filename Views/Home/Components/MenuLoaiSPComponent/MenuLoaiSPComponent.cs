using CuaHangDoAn.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuaHangDoAn.Views.Home.Components.MenuLoaiSPComponent
{
    public class MenuLoaiSPComponent: ViewComponent
    {
        private readonly CuaHangDoAnContext _context;

        public MenuLoaiSPComponent(CuaHangDoAnContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var listCategory = _context.CategoryProducts.AsNoTracking().ToList();
            return View(listCategory);
        }

    }
}
