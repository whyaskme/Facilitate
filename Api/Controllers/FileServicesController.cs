using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Facilitate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileServicesController : ControllerBase
    {
        // GET: api/<FileServicesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var tmpVal = new string[] { "value1", "value2" };

            return tmpVal;
        }

        // GET api/<FileServicesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var tmpVal = "value";

            return "value";
        }

        // POST api/<FileServicesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var tmpVal = "value";
        }

        // PUT api/<FileServicesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            var tmpVal = "value";
        }

        // DELETE api/<FileServicesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var tmpVal = "value";
        }
    }
}
