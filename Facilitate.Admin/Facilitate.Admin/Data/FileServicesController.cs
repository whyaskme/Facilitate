using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Facilitate.Admin.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileServicesController : ControllerBase
    {
        // GET: api/<FileServicesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<FileServicesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FileServicesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var tmpVal = "";
        }

        // PUT api/<FileServicesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            var tmpVal = "";
        }

        // DELETE api/<FileServicesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var tmpVal = "";
        }
    }
}
