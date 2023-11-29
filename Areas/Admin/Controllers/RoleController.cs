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
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NormalizedName,ConcurrencyStamp")] AppRole appRole)
        {
            if (ModelState.IsValid)
            {

                var newRole = new IdentityRole(appRole.Name);
                await _roleManager.CreateAsync(newRole);
                return RedirectToAction(nameof(Index));
            }
            return View(appRole);
        }



        // GET: Admin/Role/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var appRole = await _roleManager.FindByIdAsync(id);
               
            if (appRole == null)
            {
                return NotFound();
            }

            return View(appRole);
        }

        // POST: Admin/Role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            //if (_roleManager.FindByIdAsync(id)== null)
            //{
            //    return Problem("Entity set 'CuaHangDoAnContext.AppRole'  is null.");
            //}
            var appRole = await _roleManager.FindByIdAsync(id); 
            if (appRole != null)
            {
                await _roleManager.DeleteAsync(appRole);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
