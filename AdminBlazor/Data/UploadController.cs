using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminBlazor.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost("[action]")]
        public ActionResult Upload(IFormFile myFile)
        {
            try
            {
                // Write code that saves the 'myFile' file.
                // Don't rely on or trust the FileName property without validation.
                var tmpVal = "";
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
