using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dotteam.Controllers
{
    public class UploadImage 
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _uploadDirectory;
        private readonly string[] _permittedExtensions = { ".jpg", ".png", ".jpeg" };
        private readonly long _fileSizeLimit;

        public IFormFile imageFile;
        public string fileExt;

        public UploadImage(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _uploadDirectory = Path.Combine(_env.WebRootPath, "images/upload");
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
            Directory.CreateDirectory(_uploadDirectory);
        }

        public async Task<string> Create(IFormFile img)
        {
            if (img == null || img.Length <= 0)
            {
                return "Error: No file was recieved!";
            }

            imageFile = img;
            fileExt = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            string fullFilePath;
            string fileName;

            if (string.IsNullOrEmpty(fileExt) || !_permittedExtensions.Contains(fileExt))
            {
                return "Error : Invalid File Extension";
            }

            if (imageFile.Length > _fileSizeLimit)
            {
                return "Error : File max size must be 10MB";
            }

            do
            {
                fileName = Guid.NewGuid().ToString() + this.fileExt;
                fullFilePath = string.Format("{0}/{1}", _uploadDirectory, fileName);
            } while (System.IO.File.Exists(fullFilePath));

            using (var stream = System.IO.File.Create(fullFilePath))
            {
                await imageFile.CopyToAsync(stream);
            }

            return string.Format("{0}/{1}", "images/upload", fileName);
        }

        public async Task<string> Edit(string localPath, IFormFile img)
        {
            if (img == null || img.Length <= 0)
            {
                return "Error: No file was recieved!";
            }

            imageFile = img;
            fileExt = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            string fileName = localPath.Split("/").Last();
            string fullFilePath = string.Format("{0}/{1}", _uploadDirectory, fileName);

            if (string.IsNullOrEmpty(fileExt) || !_permittedExtensions.Contains(fileExt))
            {
                return "Error : Invalid File Extension";
            }

            if (imageFile.Length > _fileSizeLimit)
            {
                return "Error : File max size must be 10MB";
            }

            using (var stream = System.IO.File.Create(fullFilePath))
            {
                await imageFile.CopyToAsync(stream);
            }

            return string.Format("{0}/{1}", "images/upload", fileName);
        }

        public bool Delete(string localPath)
        {
            if (localPath == null)
            {
                return false;
            }

            string fileName = localPath.Split("/").Last();
            string fullFilePath = string.Format("{0}/{1}", _uploadDirectory, fileName);

            if (System.IO.File.Exists(fullFilePath))
            {
                System.IO.File.Delete(fullFilePath);
                return true;
            }

            return false;
        }
    }
}
