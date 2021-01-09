using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dotteam.Data;
using Dotteam.Models;

namespace Dotteam.Controllers
{
    public class ProjectController : Controller
    {
        private readonly DotteamContext _context;

        public ProjectController(DotteamContext context)
        {
            _context = context;
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

            var projectModel = await _context.ProjectModel
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
            return View();
        }

        // POST: Project/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] ProjectModel projectModel)
        {
            if (ModelState.IsValid)
            {
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

            var projectModel = await _context.ProjectModel.FindAsync(id);
            if (projectModel == null)
            {
                return NotFound();
            }
            return View(projectModel);
        }

        // POST: Project/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] ProjectModel projectModel)
        {
            if (id != projectModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
