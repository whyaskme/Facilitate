using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Facilitate.Admin.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class Upload : ControllerBase
    {
        // GET: api/<Upload>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Upload>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Upload>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var tmpVal = "value";
        }

        // PUT api/<Upload>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            var tmpVal = "value";
        }

        // DELETE api/<Upload>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var tmpVal = "value";
        }
    }
}
