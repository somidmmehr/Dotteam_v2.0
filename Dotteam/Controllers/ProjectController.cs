using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dotteam.Data;
using Dotteam.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dotteam.Controllers
{
    public class ProjectController : Controller
    {
        private readonly DotteamContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        [TempData]
        public string Message { get; set; }

        public ProjectController(DotteamContext context, IWebHostEnvironment env, IConfiguration config)
        {
            _context = context;
            _env = env;
            _config = config;
        }

        // GET: Project
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProjectModel.ToListAsync());
        }

        // GET: Project/Details/5
        public async Task<IActionResult> Details(int? id, string name)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel.Include(y => y.Teches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // GET: Project/Create
        public IActionResult Create()
        {
            ViewBag.Teches = _context.TechModel.ToList();
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DescriptionShort,DescriptionLong")] ProjectModel projectModel,
                                                IFormFile imageFile,
                                                [Bind("techIds")] int[] techIds)
        {
            if (ModelState.IsValid)
            {
                var UploadImage = new UploadImage(_env, _config);
                projectModel.Image = await UploadImage.Create(imageFile);
                if (techIds != null)
                {
                    foreach (int techId in techIds)
                    {
                        var tech = _context.TechModel.First(t => t.Id == techId);
                        if(tech != null)
                            projectModel.Teches.Add(tech);
                    }
                }
                _context.Add(projectModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projectModel);
        }

        // GET: Project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel.Include(y => y.Teches).FirstOrDefaultAsync(p => p.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }
            ViewBag.Teches = _context.TechModel.ToList();
            return View(projectModel);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
                                              [Bind("Id,Name,DescriptionShort,DescriptionLong,Image")] ProjectModel projectModel,
                                              IFormFile imageFile,
                                              [Bind("techIds")] int[] techIds)
        {
            if (id != projectModel.Id)
            {
                return NotFound();
            }
            projectModel = await _context.ProjectModel.Include(t => t.Teches).FirstOrDefaultAsync(p => p.Id == id);

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    var UploadImage = new UploadImage(_env, _config);
                    if (projectModel.Image == null)
                    {
                        projectModel.Image = await UploadImage.Create(imageFile);
                    }
                    else
                    {
                        projectModel.Image = await UploadImage.Edit(projectModel.Image, imageFile);
                    }
                }

                try
                {
                    
                    foreach (var tech in projectModel.Teches.ToList())
                    {
                        if (!techIds.Contains(tech.Id))
                            projectModel.Teches.Remove(tech);
                    }

                    foreach (int techId in techIds)
                    {
                        if (!projectModel.Teches.Any(t => t.Id == techId))
                        {
                            var tech = _context.TechModel.First(t => t.Id == techId);
                            projectModel.Teches.Add(tech);
                        }
                    }

                    _context.Update(projectModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectModelExists(projectModel.Id))
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
            return View(projectModel);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectModel = await _context.ProjectModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectModel == null)
            {
                return NotFound();
            }

            return View(projectModel);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectModel = await _context.ProjectModel.FindAsync(id);
            if (projectModel.Image != null)
            {
                var UploadImage = new UploadImage(_env, _config);
                UploadImage.Delete(projectModel.Image);
            }
            _context.ProjectModel.Remove(projectModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectModelExists(int id)
        {
            return _context.ProjectModel.Any(e => e.Id == id);
        }
    }
}
