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
    public class AddressOnlyController : ControllerBase
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

        public AddressOnlyController(DBService dBService, Utils utils)
        {
            _mongoDatabase = dBService.MongoDatabase;
            _quoteCollection = _mongoDatabase.GetCollection<Quote>(_mongoDBCollectionName);
            this.utils = utils;
        }

        [ProducesResponseType<String>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuoteAddressOnlyRoofleSubmission roofleSubmission, CancellationToken ct)
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
            quote.status = "Address Only";

            // Will need to figure out how to set dynamically
            quote.applicationType = "Roofing";

            quote.ipAddress = headerForwardedFor;
            quote.externalUrl = headerReferer;

            quote.fullAddress = roofleSubmission.address.Trim();
            quote.sessionId = roofleSubmission.sessionId;

            var tmpAddress = roofleSubmission.address.Split(",");

            quote.address = tmpAddress[0].Trim();
            quote.street = tmpAddress[0].Trim();
            quote.city = tmpAddress[1].Trim();

            var tmpStateZip = tmpAddress[2].Split(" ");

            var stateAbbreviation = await utils.GetStateAbbrByNameAsync(tmpStateZip[1], ct);

            quote.state = stateAbbreviation;
            quote.zip = tmpStateZip[2];

            quote.timestamp = DateTime.UtcNow;

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
        //[Produces("application/json")]
        //[ProducesResponseType<IEnumerable<Quote>>(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[HttpGet(Name = "Get")]
        //public IActionResult Get(string status)
        //{
        //    status = utils.TitleCaseString(status);

        //    try
        //    {
        //        var builder = Builders<Quote>.Filter;
        //        var filter = builder.Eq(f => f.status, status);

        //        sortedQuotes = _quoteCollection.Find(filter).SortByDescending(e => e.timestamp).ToList();

        //        for (var i = 0; i < sortedQuotes.Count; i++)
        //        {
        //            unSortedFiles.Clear();
        //            unSortedNotes.Clear();
        //            unSortedEvents.Clear();

        //            for (var j = 0; j < sortedQuotes[i].attachments.Count; j++)
        //            {
        //                Attachment currentAttachment = sortedQuotes[i].attachments[j];

        //                var currentDateTime = currentAttachment.Date;

        //                currentAttachment.Date = currentAttachment.Date.ToLocalTime();

        //                unSortedFiles.Add(currentAttachment);
        //            }

        //            for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
        //            {
        //                Note currentNote = sortedQuotes[i].notes[j];
        //                currentNote.Date = currentNote.Date.ToLocalTime();

        //                unSortedNotes.Add(currentNote);
        //            }

        //            for (var j = 0; j < sortedQuotes[i].events.Count; j++)
        //            {
        //                Event currentEvent = sortedQuotes[i].events[j];
        //                currentEvent.DateTime = currentEvent.DateTime.ToLocalTime();

        //                unSortedEvents.Add(currentEvent);
        //            }

        //            sortedQuotes[i].attachments = SortFilesByDateDesc(unSortedFiles);
        //            sortedQuotes[i].notes = SortNotesByDateDesc(unSortedNotes);
        //            sortedQuotes[i].events = SortEventsByDateDesc(unSortedEvents);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMsg = ex.Message;
        //        return BadRequest(resultMsg);
        //    }
        //    finally
        //    {

        //    }

        //    return sortedQuotes == null ? NotFound() : Ok(sortedQuotes);
        //}

        //private List<Event> SortEventsByDateDesc(List<Event> originalList)
        //{
        //    return originalList.OrderByDescending(x => x.DateTime).ToList();
        //}

        //private List<Attachment> SortFilesByDateDesc(List<Attachment> originalList)
        //{
        //    return originalList.OrderByDescending(x => x.Date).ToList();
        //}

        //private List<Note> SortNotesByDateDesc(List<Note> originalList)
        //{
        //    return originalList.OrderByDescending(x => x.Date).ToList();
        //}
    }
}