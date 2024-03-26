using Microsoft.AspNetCore.Mvc;

using AdminBlazor.Components.Pages;
using DevExpress.Export.Xl;
using Facilitate.Libraries.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using System.Reactive;
using System.Threading;

using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdminBlazor.Data
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileService : ControllerBase
    {
        string resultMsg = "";

        // GET: api/<FileService>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FileService>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FileService>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            resultMsg = "";
        }

        // PUT api/<FileService>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            resultMsg = "";
        }

        // DELETE api/<FileService>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            resultMsg = "";
        }
    }
}
