using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly ILogger<QuotesController> _logger;

        public QuotesController(ILogger<QuotesController> logger)
        {
            _logger = logger;
        }


        [HttpGet(Name = "GetQuotes")]
        public IEnumerable<Quote> Get()
        {
            List<Quote> quotes = new List<Quote>();
            int numQuotes = 1;

            for(var i = 0; i < numQuotes; i++)
            {
                var _quote = new Quote();

                quotes.Add(_quote);
            }

            return quotes;
        }

        [HttpPost(Name = "PostQuote")]
        public string Post(Quote quote)
        {
            var results = "New quote (" + quote._id + ") POSTED";

            return results;
        }

        [HttpPut(Name = "PutQuote")]
        public string Get(Quote quote)
        {
            var results = "Old quote (" + quote._id + ") UPDATED";

            return results;
        }

        [HttpDelete(Name = "DeleteQuote")]
        public string Delete(string quoteId)
        {
            var results = "Old quote (" + quoteId + ") DELETED";

            return results;
        }
    }
}
