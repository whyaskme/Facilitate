using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Facilitate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
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

        public QuoteController()
        {
            _mongoDBClient = new MongoClient(_mongoDBConnectionString);
            _quoteCollection = _mongoDBClient.GetDatabase(_mongoDBName).GetCollection<Quote>(_mongoDBCollectionName);
        }

        [ProducesResponseType<String>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public IActionResult Post([FromBody] QuoteRoofleSubmission roofleSubmission)
        {
            string headerForwardedFor = "n/a";
            string headerReferer = "n/a";

            var requestHeaders = HttpContext.Request.Headers;
            foreach(var header in requestHeaders)
            {
                if(header.Key == "X-Forwarded-For")
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
            quote.applicationType = utils.TitleCaseString("Roofing");

            quote.ipAddress = headerForwardedFor;
            quote.externalUrl = headerReferer;

            if (roofleSubmission.products[0].priceInfo.total != null)
            {
                quote.totalQuote = roofleSubmission.products[0].priceInfo.total;
            }
            else
            {
                quote.totalQuote = 0;
            }

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

            quote.repName = roofleSubmission.repName;
            quote.repEmail = roofleSubmission.repEmail;
            quote.leadId = roofleSubmission.leadId;

            quote.products = roofleSubmission.products;
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

        // GET: api/<QuoteController>
        [Produces("application/json")]
        [ProducesResponseType<IEnumerable<Quote>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(Name = "Get")]
        public IActionResult Get(string status)
        {
            status = utils.TitleCaseString(status);

            try
            {
                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f.status, status);

                sortedQuotes = _quoteCollection.Find(filter).SortByDescending(e => e.timestamp).ToList();

                for (var i = 0; i < sortedQuotes.Count; i++)
                {
                    unSortedFiles.Clear();
                    unSortedNotes.Clear();
                    unSortedEvents.Clear();

                    for (var j = 0; j < sortedQuotes[i].attachments.Count; j++)
                    {
                        Attachment currentAttachment = sortedQuotes[i].attachments[j];

                        var currentDateTime = currentAttachment.Date;

                        currentAttachment.Date = currentAttachment.Date.ToLocalTime();

                        unSortedFiles.Add(currentAttachment);
                    }

                    for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
                    {
                        Note currentNote = sortedQuotes[i].notes[j];
                        currentNote.Date = currentNote.Date.ToLocalTime();

                        unSortedNotes.Add(currentNote);
                    }

                    for (var j = 0; j < sortedQuotes[i].events.Count; j++)
                    {
                        Event currentEvent = sortedQuotes[i].events[j];
                        currentEvent.DateTime = currentEvent.DateTime.ToLocalTime();

                        unSortedEvents.Add(currentEvent);
                    }

                    sortedQuotes[i].attachments = SortFilesByDateDesc(unSortedFiles);
                    sortedQuotes[i].notes = SortNotesByDateDesc(unSortedNotes);
                    sortedQuotes[i].events = SortEventsByDateDesc(unSortedEvents);
                }
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
                return BadRequest(resultMsg);
            }
            finally
            {

            }

            return sortedQuotes == null ? NotFound() : Ok(sortedQuotes);
        }

        private List<Event> SortEventsByDateDesc(List<Event> originalList)
        {
            return originalList.OrderByDescending(x => x.DateTime).ToList();
        }

        private List<Attachment> SortFilesByDateDesc(List<Attachment> originalList)
        {
            return originalList.OrderByDescending(x => x.Date).ToList();
        }

        private List<Note> SortNotesByDateDesc(List<Note> originalList)
        {
            return originalList.OrderByDescending(x => x.Date).ToList();
        }
    }
}