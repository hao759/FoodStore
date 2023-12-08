using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangDoAn.Data;
using CuaHangDoAn.Models;

namespace CuaHangDoAn.Controllers
{
    //[Comment Feature]
    public class ProductsCommentsController : Controller
    {
        private readonly CuaHangDoAnContext _context;

        public ProductsCommentsController(CuaHangDoAnContext context)
        {
            _context = context;
        }

        // GET: ProductsComments
        public async Task<IActionResult> Index()
        {
            var cuaHangDoAnContext = _context.ProductsComments.Include(p => p.Product);
            return View(await cuaHangDoAnContext.ToListAsync());
        }

        // GET: ProductsComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductsComments == null)
            {
                return NotFound();
            }

            var productsComments = await _context.ProductsComments
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productsComments == null)
            {
                return NotFound();
            }

            return View(productsComments);
        }

        // GET: ProductsComments/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Products, "Id", "Name");
            return RedirectToAction("Index","Home");
        }

        // POST: ProductsComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comment,CreatedDate,Rating,ProductID")] ProductsComments productsComments, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productsComments);
                DateTime today = DateTime.Now;
                productsComments.CreatedDate = today;
                await _context.SaveChangesAsync();
                return LocalRedirect(returnUrl);
            }
            return LocalRedirect(returnUrl);
        }

        // GET: ProductsComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ProductsComments == null)
            {
                return NotFound();
            }

            var productsComments = await _context.ProductsComments.FindAsync(id);
            if (productsComments == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Products, "Id", "Name", productsComments.ProductID);
            return View(productsComments);
        }

        // POST: ProductsComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment,CreatedDate,Rating,ProductID")] ProductsComments productsComments)
        {
            if (id != productsComments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productsComments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsCommentsExists(productsComments.Id))
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
            ViewData["ProductID"] = new SelectList(_context.Products, "Id", "Name", productsComments.ProductID);
            return View(productsComments);
        }

        // GET: ProductsComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ProductsComments == null)
            {
                return NotFound();
            }

            var productsComments = await _context.ProductsComments
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productsComments == null)
            {
                return NotFound();
            }

            return View(productsComments);
        }

        // POST: ProductsComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ProductsComments == null)
            {
                return Problem("Entity set 'CuaHangDoAnContext.ProductsComments'  is null.");
            }
            var productsComments = await _context.ProductsComments.FindAsync(id);
            if (productsComments != null)
            {
                _context.ProductsComments.Remove(productsComments);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsCommentsExists(int id)
        {
          return (_context.ProductsComments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
