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
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Dotteam.Controllers
{
    public class PresentationController : Controller
    {
        private readonly DotteamContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string _uploadDirectory;
        private readonly string[] _permittedExtensions = { ".jpg", ".png", ".jpeg" };
        private readonly long _fileSizeLimit;

        [TempData]
        public string Message { get; set; }

        public PresentationController(DotteamContext context, IWebHostEnvironment env, IConfiguration config)
        {
            _context = context;
            _env = env;
            _uploadDirectory = Path.Combine(_env.WebRootPath, @"images\upload");
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
        }

        // GET: Presentaion
        public async Task<IActionResult> Index()
        {
            var dotteamContext = _context.PresentaionModel.Include(p => p.Project);
            return View(await dotteamContext.ToListAsync());
        }

        // GET: Presentaion/Details/5
        public async Task<IActionResult> Details(int? id, string name)
        {
            if (id == null || name == null)
            {
                return NotFound();
            }

            var presentaionModel = await _context.PresentaionModel
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presentaionModel == null || presentaionModel.Name.Replace(" ", "-") != name)
            {
                return NotFound();
            }
            ViewBag.Comments = _context.CommentModel.Where(x => x.PresentationId == presentaionModel.Id).OrderBy(x => x.CreatedAt).ToList();
            return View(presentaionModel);
        }

        public async Task<IActionResult> LoadComments(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentaionModel = await _context.PresentaionModel
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presentaionModel == null)
            {
                return NotFound();
            }
            var Comments = _context.CommentModel.Where(x => x.PresentationId == presentaionModel.Id).OrderBy(x => x.CreatedAt).ToList();
            return PartialView("_CommentSection", Comments);
        }

        // GET: Presentaion/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name");
            return View();
        }

        // POST: Presentaion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,DescriptionShort,DescriptionLong,EstimateTime,EstimatePrice,LastChange,ProjectId")] PresentaionModel presentaionModel,
                                                IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                string fullFilePath = null;
                string fileName = null;
                string uploadDirectory = _uploadDirectory;
                System.IO.Directory.CreateDirectory(uploadDirectory);

                if (imageFile != null && imageFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

                    if (string.IsNullOrEmpty(fileExt) || !_permittedExtensions.Contains(fileExt))
                    {
                        Message = "Error : Invalid File Extension";
                        ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", presentaionModel.ProjectId);
                        return View();
                    }

                    if (imageFile.Length > _fileSizeLimit)
                    {
                        Message = "Error : File max size must be 10MB";
                        ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", presentaionModel.ProjectId);
                        return View();
                    }

                    do
                    {
                        fileName = Guid.NewGuid().ToString() + fileExt;
                        fullFilePath = string.Format(@"{0}\{1}", uploadDirectory, fileName);
                    } while (System.IO.File.Exists(fullFilePath));

                    using (var stream = System.IO.File.Create(fullFilePath))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    presentaionModel.Image = string.Format(@"{0}\{1}", @"images\upload", fileName);
                }

                presentaionModel.LastChange = DateTime.Now;
                _context.Add(presentaionModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", presentaionModel.ProjectId);
            return View(presentaionModel);
        }

        // GET: Presentaion/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentaionModel = await _context.PresentaionModel.FindAsync(id);
            if (presentaionModel == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", presentaionModel.ProjectId);
            return View(presentaionModel);
        }

        // POST: Presentaion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id,
                                              [Bind("Id,Name,Image,DescriptionShort,DescriptionLong,EstimateTime,EstimatePrice,LastChange,ProjectId")] PresentaionModel presentaionModel,
                                              IFormFile imageFile)
        {
            if (id != presentaionModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string fileName = null;
                string fullFilePath = null;
                string uploadDirectory = _uploadDirectory;
                System.IO.Directory.CreateDirectory(uploadDirectory);

                if (imageFile != null && imageFile.Length > 0)
                {
                    string fileExt = Path.GetExtension(imageFile.FileName);

                    if (string.IsNullOrEmpty(fileExt) || !_permittedExtensions.Contains(fileExt))
                    {
                        Message = "Error : Invalid File Extension";
                        ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", presentaionModel.ProjectId);
                        return View();
                    }

                    if (imageFile.Length > _fileSizeLimit)
                    {
                        Message = "Error : File max size must be 10MB";
                        ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", presentaionModel.ProjectId);
                        return View();
                    }

                    if (presentaionModel.Image == null) 
                    {
                        do
                        {
                            fileName = Guid.NewGuid().ToString() + fileExt;
                            fullFilePath = string.Format(@"{0}\{1}", uploadDirectory, fileName);
                        } while (System.IO.File.Exists(fullFilePath));

                        presentaionModel.Image = string.Format(@"{0}\{1}", @"images\upload", fileName);
                    }
                    else
                    {
                        fullFilePath = string.Format(@"{0}\{1}", _env.WebRootPath, presentaionModel.Image);
                    }

                    using (var stream = System.IO.File.Create(fullFilePath))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                }

                try
                {
                    presentaionModel.LastChange = DateTime.Now;
                    _context.Update(presentaionModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PresentaionModelExists(presentaionModel.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.ProjectModel, "Id", "Name", presentaionModel.ProjectId);
            return View(presentaionModel);
        }

        // GET: Presentaion/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var presentaionModel = await _context.PresentaionModel
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (presentaionModel == null)
            {
                return NotFound();
            }

            return View(presentaionModel);
        }

        // POST: Presentaion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var presentaionModel = await _context.PresentaionModel.FindAsync(id);
            if(presentaionModel.Image != null)
            {
                string fullFilePath = string.Format(@"{0}\{1}", _env.WebRootPath, presentaionModel.Image);
                System.IO.File.Delete(fullFilePath);
            }
            _context.PresentaionModel.Remove(presentaionModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PresentaionModelExists(int id)
        {
            return _context.PresentaionModel.Any(e => e.Id == id);
        }
    }
}
