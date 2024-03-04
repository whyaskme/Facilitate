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
        public IEnumerable<Quote> GetQuotes()
        {
            List<Quote> quotes = new List<Quote>();
            int numQuotes = 5;

            try
            {
                for (var i = 0; i < numQuotes; i++)
                {
                    var _quote = new Quote();

                    _quote.Consumer.Title = "Mr";
                    _quote.Consumer.FirstName = "John (" + i + ")";
                    _quote.Consumer.MiddleName = "Q (" + i + ")";
                    _quote.Consumer.LastName = "Public (" + i + ")";
                    _quote.Consumer.Suffix = "Jr (" + i + ")";

                    _quote.Consumer.Contact.Email.UserName = "joe (" + i + ")";
                    _quote.Consumer.Contact.Email.Domain = "facilitate.org";

                    _quote.Consumer.Contact.Phone.AreaCode = 512;
                    _quote.Consumer.Contact.Phone.Exchange = 799;
                    _quote.Consumer.Contact.Phone.Number = 2522;

                    _quote.PropertyInfo.Address.Address1 = "123 Main St";
                    _quote.PropertyInfo.Address.City = "Austin";
                    _quote.PropertyInfo.Address.State = "TX";
                    _quote.PropertyInfo.Address.ZipCode = 78753;
                    _quote.PropertyInfo.Address.Country = "USA";


                    quotes.Add(_quote);
                }
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {

            }

            return quotes;
        }

        [HttpPost(Name = "PostQuote")]
        public string PostQuote(Quote quote)
        {
            var results = string.Empty;

            try
            {

            }
            catch (Exception ex)
            {
                results = ex.Message;
            }
            finally
            {
                results = "New quote (" + quote._id + ") for (" + quote.Consumer.FirstName + " " + quote.Consumer.LastName + ") POSTED";
            }

            return results;
        }

        [HttpPut(Name = "PutQuote")]
        public string PutQuote(Quote quote)
        {
            var results = string.Empty;

            try
            {

            }
            catch (Exception ex)
            {
                results = ex.Message;
            }
            finally
            {
                results = "Old quote (" + quote._id + ") for (" + quote.Consumer.FirstName + " " + quote.Consumer.LastName + ") UPDATED";
            }

            return results;
        }

        [HttpDelete(Name = "DeleteQuote")]
        public string DeleteQuote(string quoteId)
        {
            var results = string.Empty;

            try
            {

            }
            catch (Exception ex)
            {
                results = ex.Message;
            }
            finally
            {
                results = "Old quote (" + quoteId + ") DELETED";
            }

            return results;
        }
    }
}
