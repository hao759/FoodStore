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
using CuaHangDoAn.ModelView;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CuaHangDoAn.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly CuaHangDoAnContext _context;
        public RoleManager<IdentityRole> _roleManager { get; set; }

        public UserManager<AppUser> _userManager { get; private set; }

        [TempData]
        public string StatusMessage { get; set; } = "";
        public RoleController(CuaHangDoAnContext context, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager; 
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
                StatusMessage = $"Bạn vừa tạo role mới: {appRole.Name}";
                return RedirectToAction(nameof(Index));
            }
            return View(appRole);
        }


        public async Task<IActionResult> AddRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var listRole = (await _userManager.GetRolesAsync(user)).ToArray();

            AddRoleMV getuser = new AddRoleMV();
            getuser.Roles = listRole;
            getuser.Id = user.Id;
            getuser.Name = user.UserName;

            List<string> rolename= await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.SelectlistRole= rolename;
            return View(getuser);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleMV model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            var listRole = (await _userManager.GetRolesAsync(user)).ToArray();

            var OldRoleNames = (await _userManager.GetRolesAsync(user)).ToArray();

            var deleteRoles = OldRoleNames.Where(r => !model.Roles.Contains(r));
            var addRoles = model.Roles.Where(r => !OldRoleNames.Contains(r));

            List<string> roleNames = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            ViewBag.SelectlistRole = roleNames;

            var resultDelete = await _userManager.RemoveFromRolesAsync(user, deleteRoles);


            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);


            return View();
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
