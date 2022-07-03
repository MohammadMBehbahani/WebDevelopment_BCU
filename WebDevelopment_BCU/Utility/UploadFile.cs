using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace WebDevelopment_BCU.Utility
{
    public class UploadFile
    {
        private readonly IWebHostEnvironment _environment;

        public UploadFile(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public UploadDto UploadFileFunction(IFormFile file, string path)
        {
            if (file != null && file.Length > 0)
            {
                string folder = $@"Img\" + path;
                var uploadsRootFolder = Path.Combine(_environment.WebRootPath, folder);
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }




                string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(uploadsRootFolder, fileName);
                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

                return new UploadDto()
                {
                    FileNameAddress = folder + fileName,
                    Status = true,
                    FileName = fileName,
                };
            }
            return new UploadDto()
            {
                Status = false,
                FileNameAddress = "",
                FileName = ""
            };
        }
    }
}
