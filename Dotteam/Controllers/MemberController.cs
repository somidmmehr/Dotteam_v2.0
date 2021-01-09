using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotteam.Data;
using Dotteam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dotteam.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MemberController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        [TempData]
        public string StatusMessage { get; set; }

        public MemberController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: MemberController
        public ActionResult Index()
        {
            return View();
        }

        // GET: MemberController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MemberController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MemberController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MemberController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MemberController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MemberController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Roles()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles.Select(x => new RoleViewModel() { Id = x.Id, Name = x.Name }));
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult CreateRole() 
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRole([Bind("Name")] RoleViewModel role)
        {
            if(await _roleManager.RoleExistsAsync(role.Name))
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return View();
            }
            var identityRole = new IdentityRole
            {
                Name = role.Name
            };

            var result = await _roleManager.CreateAsync(identityRole);

            if (result.Succeeded) {
                return RedirectToAction("Roles");
            }

            return View();
        }
    }
}
