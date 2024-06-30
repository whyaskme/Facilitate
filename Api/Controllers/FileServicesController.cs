using Microsoft.AspNetCore.Mvc;

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using UsingUploadEditDataGrid.Data;

using Microsoft.AspNetCore.Http;

using MongoDB.Bson;

using System.Text.Json;

using DevExpress.Blazor;
using DevExpress.Data;
using DevExpress.Data.Linq;
using DevExpress.Web;

namespace Facilitate.Api.Controllers
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
    [ApiController]
    public class FileServicesController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        FileUrlStorageService _fileUrlStorageService;

        //public FileServicesController(IWebHostEnvironment hostingEnvironment, FileUrlStorageService fileUrlStorageService)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //    _fileUrlStorageService = fileUrlStorageService;
        //}

        public FileServicesController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileUrlStorageService = new FileUrlStorageService();
        }

        // GET: api/<FileServicesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var resutMsg = new string[] { "value1", "value2" };

            return resutMsg;
        }

        // POST api/<FileServicesController>
        [HttpPost]
        public string Post([FromBody] string value)
        {
            var fileId = ObjectId.GenerateNewId().ToString();

            return fileId;
        }

        // GET api/<FileServicesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}
    }
}
