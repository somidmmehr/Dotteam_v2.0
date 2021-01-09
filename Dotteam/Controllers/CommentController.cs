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
    public class CommentController : Controller
    {
        private readonly DotteamContext _context;

        public CommentController(DotteamContext context)
        {
            _context = context;
        }

        // GET: Comment
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index()
        {
            var dotteamContext = _context.CommentModel.Include(c => c.ParentComment).Include(c => c.Presentaion);
            return View(await dotteamContext.ToListAsync());
        }

        // GET: Comment/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.CommentModel
                .Include(c => c.ParentComment)
                .Include(c => c.Presentaion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }

        // GET: Comment/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["ReplyToCommentId"] = new SelectList(_context.CommentModel, "Id", "Name");
            ViewData["PresentationId"] = new SelectList(_context.PresentaionModel, "Id", "Name");
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Title,Text,CreatedAt,State,PresentationId,ReplyToCommentId")] CommentModel commentModel)
        {
            if (ModelState.IsValid)
            {
                commentModel.CreatedAt = DateTime.Now;
                commentModel.State = "Shows";
                _context.Add(commentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReplyToCommentId"] = new SelectList(_context.CommentModel, "Id", "Name", commentModel.ReplyToCommentId);
            ViewData["PresentationId"] = new SelectList(_context.PresentaionModel, "Id", "Name", commentModel.PresentationId);
            return View(commentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax([Bind("Id,Name,Email,Title,Text,CreatedAt,State,PresentationId,ReplyToCommentId")] CommentModel commentModel)
        {
            if (ModelState.IsValid)
            {
                commentModel.CreatedAt = DateTime.Now;
                commentModel.State = "Show";
                _context.Add(commentModel);
                await _context.SaveChangesAsync();
                return Json(true);
            }
            return Json(false);
        }

        // GET: Comment/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.CommentModel.FindAsync(id);
            if (commentModel == null)
            {
                return NotFound();
            }
            ViewData["ReplyToCommentId"] = new SelectList(_context.CommentModel, "Id", "Name", commentModel.ReplyToCommentId);
            ViewData["PresentationId"] = new SelectList(_context.PresentaionModel, "Id", "Name", commentModel.PresentationId);
            return View(commentModel);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Title,Text,CreatedAt,State,PresentationId,ReplyToCommentId")] CommentModel commentModel)
        {
            if (id != commentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentModelExists(commentModel.Id))
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
            ViewData["ReplyToCommentId"] = new SelectList(_context.CommentModel, "Id", "Name", commentModel.ReplyToCommentId);
            ViewData["PresentationId"] = new SelectList(_context.PresentaionModel, "Id", "Name", commentModel.PresentationId);
            return View(commentModel);
        }

        // GET: Comment/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.CommentModel
                .Include(c => c.ParentComment)
                .Include(c => c.Presentaion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }

        // POST: Comment/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentModel = await _context.CommentModel.FindAsync(id);
            _context.CommentModel.Remove(commentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentModelExists(int id)
        {
            return _context.CommentModel.Any(e => e.Id == id);
        }
    }
}
