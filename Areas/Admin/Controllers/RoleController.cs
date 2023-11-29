using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CuaHangDoAn.Data;
using CuaHangDoAn.Models;
using Microsoft.AspNetCore.Identity;

namespace CuaHangDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly CuaHangDoAnContext _context;
        public RoleManager<IdentityRole> _roleManager { get; set; }

        public RoleController(CuaHangDoAnContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        // GET: Admin/Role
        public async Task<IActionResult> Index()
        {
            var r = await _roleManager.Roles.ToListAsync();
            return View(r);
        }

        // GET: Admin/Role/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Role/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AppRole appRole)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(appRole);
                //await _context.SaveChangesAsync();

                var newRole = new IdentityRole(appRole.Name);
                await _roleManager.CreateAsync(newRole);
                return RedirectToAction(nameof(Index));
            }
            return View(appRole);
        }

        //// GET: Admin/Role/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null || _context.AppRole == null)
        //    {
        //        return NotFound();
        //    }

        //    var appRole = await _context.AppRole.FindAsync(id);
        //    if (appRole == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(appRole);
        //}

        //// POST: Admin/Role/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AppRole appRole)
        //{
        //    if (id != appRole.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(appRole);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AppRoleExists(appRole.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(appRole);
        //}

        //// GET: Admin/Role/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null || _context.AppRole == null)
        //    {
        //        return NotFound();
        //    }

        //    var appRole = await _context.AppRole
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (appRole == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(appRole);
        //}

        //// POST: Admin/Role/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    if (_context.AppRole == null)
        //    {
        //        return Problem("Entity set 'CuaHangDoAnContext.AppRole'  is null.");
        //    }
        //    var appRole = await _context.AppRole.FindAsync(id);
        //    if (appRole != null)
        //    {
        //        _context.AppRole.Remove(appRole);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool AppRoleExists(string id)
        //{
        //  return (_context.AppRole?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
