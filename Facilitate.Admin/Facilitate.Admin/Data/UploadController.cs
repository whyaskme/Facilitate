using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using UsingUploadEditDataGrid.Data;

namespace UsingUploadEditDataGrid.Controllers 
{
    public class ChunkMetadata 
    {
        public int Index { get; set; }
        public int TotalCount { get; set; }
        public int FileSize { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? FileGuid { get; set; }
    }

    [Route("api/[controller]")]
    public class UploadController : Controller 
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        FileUrlStorageService _fileUrlStorageService;
        public UploadController(IWebHostEnvironment hostingEnvironment, FileUrlStorageService fileUrlStorageService) 
        {
            _hostingEnvironment = hostingEnvironment;
            _fileUrlStorageService = fileUrlStorageService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var resultMsg = "";

            try
            {
                resultMsg = " Quotes added";
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
                return BadRequest(resultMsg);
            }
            finally
            {

            }
            return Ok(resultMsg);

        }

        [HttpPost]
        [Route("UploadFile")]
        [DisableRequestSizeLimit]
        public ActionResult UploadFile(IFormFile ImageUpload, string chunkMetadata) 
        {
            var tempPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            // Removes temporary files
            RemoveTempFilesAfterDelay(tempPath, new TimeSpan(0, 5, 0));

            try {
                if (!string.IsNullOrEmpty(chunkMetadata)) 
                {
                    var metaDataObject = JsonSerializer.Deserialize<ChunkMetadata>(chunkMetadata);
                    var tempFilePath = Path.Combine(tempPath, metaDataObject.FileGuid + ".tmp");
                    if (!Directory.Exists(tempPath))
                        Directory.CreateDirectory(tempPath);

                    AppendContentToFile(tempFilePath, ImageUpload);

                    if (metaDataObject.Index == (metaDataObject.TotalCount - 1)) 
                    {
                        ProcessUploadedFile(tempFilePath, metaDataObject.FileName);
                        _fileUrlStorageService.Add(Guid.Parse(metaDataObject.FileGuid), @"uploads\" + metaDataObject.FileName);
                    }
                }
            }
            catch (Exception ex) {
                return BadRequest();
            }
            return Ok();
        }

        void RemoveTempFilesAfterDelay(string path, TimeSpan delay) 
        {
            var dir = new DirectoryInfo(path);
            if (dir.Exists)
                foreach (var file in dir.GetFiles("*.tmp").Where(f => f.LastWriteTimeUtc.Add(delay) < DateTime.UtcNow))
                    file.Delete();
        }

        void ProcessUploadedFile(string tempFilePath, string fileName) 
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "Uploads");

            var newFileName = "" + Guid.NewGuid().ToString() + " - " + fileName;

            var imagePath = Path.Combine(path, newFileName);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
                
            System.IO.File.Copy(tempFilePath, imagePath);

            // Cleanup
            System.IO.File.Delete(tempFilePath);
        }

        void AppendContentToFile(string path, IFormFile content) 
        {
            using (var stream = new FileStream(path, FileMode.Append, FileAccess.Write)) 
            {
                content.CopyTo(stream);
            }
        }
    }
}
