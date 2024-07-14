using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

using System.Net.Http;
using System.Web.Http.Cors;

namespace Facilitate.Api.Controllers
{
    [DisableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRequestedController : ControllerBase
    {
        Utils utils = new Utils();

        string _mongoDBName = "Facilitate";
        string _mongoDBCollectionName = "Quote";

        //string _mongoDBConnectionString = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate;safe=true;maxpoolsize=200";
        string _mongoDBConnectionString = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate;safe=true;maxpoolsize=200";

        IMongoClient _mongoDBClient;

        IMongoCollection<Quote> _quoteCollection;

        List<Quote> sortedQuotes = new List<Quote>();

        List<Attachment> unSortedFiles = new List<Attachment>();
        List<Note> unSortedNotes = new List<Note>();
        List<Event> unSortedEvents = new List<Event>();

        string resultMsg = string.Empty;

        public ProductRequestedController()
        {
            _mongoDBClient = new MongoClient(_mongoDBConnectionString);
            _quoteCollection = _mongoDBClient.GetDatabase(_mongoDBName).GetCollection<Quote>(_mongoDBCollectionName);
        }

        [ProducesResponseType<String>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public IActionResult Post([FromBody] QuoteProductRequestedRoofleSubmission roofleSubmission)
        {
            string headerForwardedFor = "n/a";
            string headerReferer = "n/a";

            var requestHeaders = HttpContext.Request.Headers;
            foreach (var header in requestHeaders)
            {
                if (header.Key == "X-Forwarded-For")
                {
                    headerForwardedFor = header.Value;
                }

                if (header.Key == "Referer")
                {
                    headerReferer = header.Value;
                }
            }

            Quote quote = new Quote();

            // Will need to figure out how to set dynamically
            quote.applicationType = "Roofing";
            quote.status = "Product Requested";

            quote.ipAddress = headerForwardedFor;
            quote.externalUrl = headerReferer;

            Product product = new Product();
            product.name = roofleSubmission.productName;

            //product.priceInfo = new PriceInfo();
            product.priceInfo = roofleSubmission.priceInfo;
            product.name = roofleSubmission.productName;
            product.priceRange = roofleSubmission.priceRange;


            quote.products.Add(product);

            quote.address = roofleSubmission.address;
            quote.fullAddress = roofleSubmission.fullAddress;
            quote.street = roofleSubmission.street;
            quote.city = roofleSubmission.city;

            var stateAbbreviation = utils.GetStateAbbrByName(roofleSubmission.state);

            quote.state = stateAbbreviation;
            quote.zip = roofleSubmission.zip;

            quote.firstName = roofleSubmission.firstName;
            quote.lastName = roofleSubmission.lastName;
            quote.email = roofleSubmission.email;
            quote.phone = roofleSubmission.phone;
            quote.market = roofleSubmission.market;

            quote.timestamp = DateTime.UtcNow;

            quote.numberOfStructures = roofleSubmission.numberOfStructures;
            quote.numberOfIncludedStructures = roofleSubmission.numberOfIncludedStructures;
            quote.totalSquareFeet = roofleSubmission.totalSquareFeet;

            //quote.products = roofleSubmission.products;
            quote.structures = roofleSubmission.structures;

            quote.mainRoofTotalSquareFeet = roofleSubmission.mainRoofTotalSquareFeet;
            quote.totalInitialSquareFeet = roofleSubmission.totalInitialSquareFeet;
            quote.sessionId = roofleSubmission.sessionId;

            Event _event = new Event();
            _event.Name = "New Quote";
            _event.DateTime = DateTime.UtcNow;
            _event.Details = "New quote referred by: " + headerReferer;

            var author = new ApplicationUser();
            author.Id = Guid.NewGuid().ToString();
            author.FirstName = "Web";
            author.LastName = "Api";

            _event.Author = author;

            quote.events.Add(_event);

            try
            {
                _quoteCollection = _mongoDBClient.GetDatabase(_mongoDBName).GetCollection<Quote>(_mongoDBCollectionName);
                _quoteCollection.InsertOne(quote);

                resultMsg = "Added QuoteId: " + quote._id;
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
                return BadRequest(resultMsg);
            }
            finally
            {

            }
            return Ok(resultMsg);
        }
    }
}