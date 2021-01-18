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
        private readonly string _uploadDirectory;
        private readonly string[] _permittedExtensions = { ".jpg", ".png", ".jpeg" };
        private readonly long _fileSizeLimit;

        [TempData]
        public string Message { get; set; }

        public ProjectController(DotteamContext context, IWebHostEnvironment env, IConfiguration config)
        {
            _context = context;
            _env = env;
            _uploadDirectory = Path.Combine(_env.WebRootPath, @"images\upload");
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
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
        public async Task<IActionResult> Create([Bind("Id,Name,DescriptionLong")] ProjectModel projectModel,
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
                        return RedirectToAction("Edit", projectModel);
                    }

                    if (imageFile.Length > _fileSizeLimit)
                    {
                        Message = "Error : File max size must be 10MB";
                        return View(projectModel);
                    }

                    do
                    {
                        fileName = Guid.NewGuid().ToString() + fileExt;
                        fullFilePath = string.Format(@"{0}\{1}", uploadDirectory, fileName);
                    } while (System.IO.File.Exists(fullFilePath));

                    projectModel.Image = string.Format(@"{0}\{1}", @"images\upload", fileName);

                    using (var stream = System.IO.File.Create(fullFilePath))
                    {
                        await imageFile.CopyToAsync(stream);
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
        public async Task<IActionResult> Edit(int id,
                                              [Bind("Id,Name,DescriptionShort,DescriptionLong,Image")] ProjectModel projectModel,
                                              IFormFile imageFile)
        {
            if (id != projectModel.Id)
            {
                return NotFound();
            }

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
                        return RedirectToAction("Edit",projectModel);
                    }

                    if (imageFile.Length > _fileSizeLimit)
                    {
                        Message = "Error : File max size must be 10MB";
                        return View(projectModel);
                    }

                    if (projectModel.Image == null)
                    {
                        do
                        {
                            fileName = Guid.NewGuid().ToString() + fileExt;
                            fullFilePath = string.Format(@"{0}\{1}", uploadDirectory, fileName);
                        } while (System.IO.File.Exists(fullFilePath));

                        projectModel.Image = string.Format(@"{0}\{1}", @"images\upload", fileName);
                    }
                    else
                    {
                        fullFilePath = string.Format(@"{0}\{1}", _env.WebRootPath, projectModel.Image);
                    }

                    using (var stream = System.IO.File.Create(fullFilePath))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                }

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
