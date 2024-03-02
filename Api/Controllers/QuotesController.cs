using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotesController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<QuotesController> _logger;

        public QuotesController(ILogger<QuotesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetQuotes")]
        public IEnumerable<Quote> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new Quote
            {
                //quote.Date = DateAndTime.Now,
                Address = "123 Main St",
                City = "Anytown",
                State = "CA",
                Zip = "12345",
                FirstName = "John",
                LastName = "Doe",
                Email = ""
        })
            .ToArray();

            //for (int i = 0; i < 5; i++)
            //{
            //    var quote = new Quote();
            //    quote.Date = DateAndTime.Now;
            //    quote.Address = "123 Main St";
            //    quote.City = "Anytown";
            //    quote.State = "CA";
            //    quote.Zip = "12345";
            //    quote.FirstName = "John";
            //    quote.LastName = "Doe";
            //    quote.Email = "";
            //}
        }
    }
}
