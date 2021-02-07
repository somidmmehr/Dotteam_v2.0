using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dotteam.Data;
using Dotteam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace Dotteam.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class TechController : Controller
    {
        private readonly DotteamContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public TechController(DotteamContext context, IWebHostEnvironment env, IConfiguration config)
        {
            _context = context;
            _env = env;
            _config = config;
        }

        // GET: Tech
        public async Task<IActionResult> Index()
        {
            return View(await _context.TechModel.ToListAsync());
        }

        // GET: Tech/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techModel = await _context.TechModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (techModel == null)
            {
                return NotFound();
            }

            return View(techModel);
        }

        // GET: Tech/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tech/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Description")] TechModel techModel,
                                                IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var UploadImage = new UploadImage(_env, _config);
                techModel.Image = await UploadImage.Create(imageFile);
                _context.Add(techModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(techModel);
        }

        // GET: Tech/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techModel = await _context.TechModel.FindAsync(id);
            if (techModel == null)
            {
                return NotFound();
            }
            return View(techModel);
        }

        // POST: Tech/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
                                              [Bind("Id,Name,Image,Description")] TechModel techModel,
                                              IFormFile imageFile)
        {
            if (id != techModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(imageFile != null)
                {
                    var UploadImage = new UploadImage(_env, _config);
                    if (techModel.Image == null)
                    {
                        techModel.Image = await UploadImage.Create(imageFile);
                    }
                    else
                    {
                        techModel.Image = await UploadImage.Edit(techModel.Image ,imageFile);
                    }
                }

                try
                {
                    _context.Update(techModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TechModelExists(techModel.Id))
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
            return View(techModel);
        }

        // GET: Tech/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techModel = await _context.TechModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (techModel == null)
            {
                return NotFound();
            }

            return View(techModel);
        }

        // POST: Tech/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var techModel = await _context.TechModel.FindAsync(id);
            if(techModel.Image != null)
            {
                var UploadImage = new UploadImage(_env, _config);
                UploadImage.Delete(techModel.Image);
            }
            _context.TechModel.Remove(techModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechModelExists(int id)
        {
            return _context.TechModel.Any(e => e.Id == id);
        }
    }
}
