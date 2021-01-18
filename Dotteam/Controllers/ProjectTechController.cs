using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dotteam.Data;
using Dotteam.Models;

namespace Dotteam.Controllers
{
    public class ProjectTechController : Controller
    {
        private readonly DotteamContext _context;

        public ProjectTechController(DotteamContext context)
        {
            _context = context;
        }

        // GET: ProjectTech
        public async Task<IActionResult> Index()
        {
            var dotteamContext = _context.ProjectTechModel.Include(p => p.Project);
            return View(await dotteamContext.ToListAsync());
        }

        // GET: ProjectTech/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTechModel = await _context.ProjectTechModel
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectTechModel == null)
            {
                return NotFound();
            }

            return View(projectTechModel);
        }

        // GET: ProjectTech/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name");
            return View();
        }

        // POST: ProjectTech/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Description,ProjectId")] ProjectTechModel projectTechModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projectTechModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", projectTechModel.ProjectId);
            return View(projectTechModel);
        }

        // GET: ProjectTech/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTechModel = await _context.ProjectTechModel.FindAsync(id);
            if (projectTechModel == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", projectTechModel.ProjectId);
            return View(projectTechModel);
        }

        // POST: ProjectTech/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Description,ProjectId")] ProjectTechModel projectTechModel)
        {
            if (id != projectTechModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTechModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTechModelExists(projectTechModel.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", projectTechModel.ProjectId);
            return View(projectTechModel);
        }

        // GET: ProjectTech/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectTechModel = await _context.ProjectTechModel
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projectTechModel == null)
            {
                return NotFound();
            }

            return View(projectTechModel);
        }

        // POST: ProjectTech/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projectTechModel = await _context.ProjectTechModel.FindAsync(id);
            _context.ProjectTechModel.Remove(projectTechModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTechModelExists(int id)
        {
            return _context.ProjectTechModel.Any(e => e.Id == id);
        }
    }
}
