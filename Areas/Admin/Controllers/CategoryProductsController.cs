using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangDoAn.Data;
using CuaHangDoAn.Models;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace CuaHangDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Admin)]
    public class CategoryProductsController : Controller
    {
        private readonly CuaHangDoAnContext _context;
        private INotyfService _notifyService;

        public CategoryProductsController(CuaHangDoAnContext context, INotyfService notyfService)
        {
            _context = context;
            _notifyService= notyfService;
        }

        // GET: Admin/CategoryProducts
        public async Task<IActionResult> Index()
        {
              return _context.CategoryProducts != null ? 
                          View(await _context.CategoryProducts.ToListAsync()) :
                          Problem("Entity set 'CuaHangDoAnContext.CategoryProducts'  is null.");
        }

        // GET: Admin/CategoryProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CategoryProducts == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        // GET: Admin/CategoryProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CategoryProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] CategoryProduct categoryProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryProduct);
                await _context.SaveChangesAsync();
                _notifyService.Success("Thêm thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(categoryProduct);
        }

        // GET: Admin/CategoryProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CategoryProducts == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProducts.FindAsync(id);
            if (categoryProduct == null)
            {
                return NotFound();
            }
            return View(categoryProduct);
        }

        // POST: Admin/CategoryProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] CategoryProduct categoryProduct)
        {
            if (id != categoryProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoryProduct);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Chỉnh sửa thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryProductExists(categoryProduct.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoryProduct);
        }

        // GET: Admin/CategoryProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CategoryProducts == null)
            {
                return NotFound();
            }

            var categoryProduct = await _context.CategoryProducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryProduct == null)
            {
                return NotFound();
            }

            return View(categoryProduct);
        }

        // POST: Admin/CategoryProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CategoryProducts == null)
            {
                return Problem("Entity set 'CuaHangDoAnContext.CategoryProducts'  is null.");
            }
            var categoryProduct = await _context.CategoryProducts.FindAsync(id);
            if (categoryProduct != null)
            {
                _context.CategoryProducts.Remove(categoryProduct);
            }
            
            await _context.SaveChangesAsync();
            _notifyService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryProductExists(int id)
        {
          return (_context.CategoryProducts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult Validate(string Description) {
            if (Description.Length == 2)
                return Json(data: "Bằng 2");
            return Json(true);
        }
    }
}
