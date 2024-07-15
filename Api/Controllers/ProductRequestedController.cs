using Facilitate.Libraries.Models;
using Facilitate.Libraries.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Facilitate.Api.Controllers
{
    [DisableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductRequestedController : ControllerBase
    {
        private readonly Utils utils;

        string _mongoDBCollectionName = "Quote";

        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<Quote> _quoteCollection;

        List<Quote> sortedQuotes = new List<Quote>();

        List<Attachment> unSortedFiles = new List<Attachment>();
        List<Note> unSortedNotes = new List<Note>();
        List<Event> unSortedEvents = new List<Event>();

        string resultMsg = string.Empty;

        public ProductRequestedController(DBService dBService, Utils utils)
        {
            _mongoDatabase = dBService.MongoDatabase;
            _quoteCollection = _mongoDatabase.GetCollection<Quote>(_mongoDBCollectionName);
            this.utils = utils;
        }

        [ProducesResponseType<String>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuoteProductRequestedRoofleSubmission roofleSubmission, CancellationToken ct)
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

            var stateAbbreviation = await utils.GetStateAbbrByNameAsync(roofleSubmission.state, ct);

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
                await _quoteCollection.InsertOneAsync(quote, null, ct);

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