using Facilitate.Libraries.Models;
using Facilitate.Libraries.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.AspNetCore.Cors;

namespace Facilitate.Api.Controllers
{
    [DisableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
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

        public QuoteController(DBService dBService, Utils utils)
        {
            _mongoDatabase = dBService.MongoDatabase;
            _quoteCollection = _mongoDatabase.GetCollection<Quote>(_mongoDBCollectionName);
            this.utils = utils;
        }

        [ProducesResponseType<String>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuoteRoofleSubmission roofleSubmission, CancellationToken ct)
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

        // GET: api/<QuoteController>
        [Produces("application/json")]
        [ProducesResponseType<IEnumerable<Quote>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(Name = "Get")]
        public async Task<IActionResult> Get(string status, CancellationToken ct)
        {
            status = utils.TitleCaseString(status);

            try
            {
                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f.status, status);

                sortedQuotes = await _quoteCollection.Find(filter).SortByDescending(e => e.timestamp).ToListAsync(ct);

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