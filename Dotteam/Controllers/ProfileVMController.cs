using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotteam.Data;
using Dotteam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dotteam.Controllers
{
    public class ProfileVMController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [TempData]
        public string StatusMessage { get; set; }

        public ProfileVMController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: ProfileVMController
        public async Task<ActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var currentRole = await _userManager.GetRolesAsync(currentUser);
            if (currentUser == null)
            {
                return NotFound();
            }
            var profile = new ProfileViewModel
            {
                Name = currentUser.NormalizedUserName,
                Email = currentUser.Email,
                Address = currentUser.Address,
                Role = ""
            };
            return View(profile);
        }

        public ActionResult Role()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles.Select(x => new RoleViewModel() { Id = x.Id, Name = x.Name }));
        }

        public async Task<IActionResult> Edit()
        {

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound();
            }

            var profile = new ProfileViewModel
            {
                Name = currentUser.NormalizedUserName,
                Email = currentUser.Email,
                Address = currentUser.Address
            };

            return View(profile);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Name,Email,Address")] ApplicationUser user)
        {
            
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                currentUser.Address = user.Address;
                await _userManager.UpdateAsync(currentUser);
                StatusMessage = "Your profile has been updated";
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
