using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangDoAn.Data;
using CuaHangDoAn.Models;
using Microsoft.Extensions.Hosting;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace CuaHangDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly CuaHangDoAnContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public INotyfService _notifyService { get; }
        public ProductsController(CuaHangDoAnContext context, IWebHostEnvironment hostEnvironment, INotyfService notifyService)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _notifyService = notifyService;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var cuaHangDoAnContext = _context.Products.Include(p => p.CategoryProduct);
            return View(await cuaHangDoAnContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.CategoryProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["CateID"] = new SelectList(_context.CategoryProducts, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CateID,ImageFile,Price")] Product product)
        {
            ViewData["CateID"] = new SelectList(_context.CategoryProducts, "Id", "Name", product.CateID);
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);               
                product.Image = fileName+ DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", product.Image);//thíu extension tên file thíu .png
                //product.Description = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(fileStream);
                }
                

                _context.Add(product);
                await _context.SaveChangesAsync();
                _notifyService.Success("Thêm sản phẩm thành công");
                return RedirectToAction(nameof(Index));
            }
           
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CateID"] = new SelectList(_context.CategoryProducts, "Id", "Name", product.CateID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CateID,ImageFile,Image,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(product.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        System.IO.File.Delete(wwwRootPath + "/Image/" + product.Image);

                        string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                        string extension = Path.GetExtension(product.ImageFile.FileName);
                        product.Image = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/Image/", product.Image);//thíu extension tên file thíu .png
                                                                                           //product.Description = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                       
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _notifyService.Success("Chỉnh sửa sản phẩm thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CateID"] = new SelectList(_context.CategoryProducts, "Id", "Name", product.CateID);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.CategoryProduct)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id,string Image)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'CuaHangDoAnContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string path = Path.Combine(wwwRootPath + "/Image/", Image);//thíu extension tên file thíu .png

                System.IO.File.Delete(path);
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
