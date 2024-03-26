using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorDemo.AspNetCoreHost;
[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    [HttpGet("[action]")]
    public ActionResult Upload()
    {
        try
        {
            // Write code that saves the 'myFile' file.
            // Don't rely on or trust the FileName property without validation.
            var tmpVal = "";
        }
        catch(Exception ex)
        {
            var errMsg = ex.Message;

            return BadRequest();
        }
        return Ok();
    }

    [HttpPost("[action]")]
    public ActionResult Upload(IFormFile myFile)
    {
        try
        {
            // Write code that saves the 'myFile' file.
            // Don't rely on or trust the FileName property without validation.
            var tmpVal = "";
        }
        catch (Exception ex)
        {
            var errMsg = ex.Message;

            return BadRequest();
        }
        return Ok();
    }
}