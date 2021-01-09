using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dotteam.Data;
using Dotteam.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dotteam.Controllers
{
    [Authorize(Roles = "Admin")]
    public class IssueController : Controller
    {
        private readonly DotteamContext _context;

        public IssueController(DotteamContext context)
        {
            _context = context;
        }

        // GET: Issue
        public async Task<IActionResult> Index()
        {
            var dotteamContext = _context.IssueModel.Include(i => i.Project);
            return View(await dotteamContext.ToListAsync());
        }

        // GET: Issue/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var issueModel = await _context.IssueModel
                .Include(i => i.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issueModel == null)
            {
                return NotFound();
            }
            
            return View(issueModel);
        }

        // GET: Issue/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name");
            return View();
        }

        // POST: Issue/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ProjectId")] IssueModel issueModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(issueModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", issueModel.ProjectId);
            return View(issueModel);
        }

        // GET: Issue/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var issueModel = await _context.IssueModel.FindAsync(id);
            if (issueModel == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", issueModel.ProjectId);
            return View(issueModel);
        }

        // POST: Issue/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ProjectId")] IssueModel issueModel)
        {
            if (id != issueModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issueModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssueModelExists(issueModel.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", issueModel.ProjectId);
            return View(issueModel);
        }

        // GET: Issue/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var issueModel = await _context.IssueModel
                .Include(i => i.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issueModel == null)
            {
                return NotFound();
            }

            return View(issueModel);
        }

        // POST: Issue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var issueModel = await _context.IssueModel.FindAsync(id);
            _context.IssueModel.Remove(issueModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssueModelExists(int id)
        {
            return _context.IssueModel.Any(e => e.Id == id);
        }
    }
}
