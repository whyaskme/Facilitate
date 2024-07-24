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

        public List<Quote> newQuoteListQueue = new List<Quote>();

        [ProducesResponseType<String>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public IActionResult Post([FromBody] QuoteRoofleSubmission roofleSubmission)
        {
            try
            {
                string headerForwardedFor = "n/a";
                string headerReferer = "n/a";

                int childQuotesToCreate = 1;
                int biddingExpiresInDays = 5;

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

                Quote aggregateQuote = new Quote();
                aggregateQuote.applicationType = utils.TitleCaseString("Aggregate");

                // Set expiration
                aggregateQuote.biddingExpires = DateTime.UtcNow.AddDays(biddingExpiresInDays);

                aggregateQuote.ipAddress = headerForwardedFor;
                aggregateQuote.externalUrl = headerReferer;

                if (roofleSubmission.products[0].priceInfo.total != null)
                {
                    aggregateQuote.totalQuote = roofleSubmission.products[0].priceInfo.total;
                }
                else
                {
                    aggregateQuote.totalQuote = 0;
                }

                aggregateQuote.address = roofleSubmission.address;
                aggregateQuote.fullAddress = roofleSubmission.fullAddress;
                aggregateQuote.street = roofleSubmission.street;
                aggregateQuote.city = roofleSubmission.city;

                var stateAbbreviation = utils.GetStateAbbrByName(roofleSubmission.state);

                aggregateQuote.state = stateAbbreviation;
                aggregateQuote.zip = roofleSubmission.zip;

                aggregateQuote.firstName = roofleSubmission.firstName;
                aggregateQuote.lastName = roofleSubmission.lastName;
                aggregateQuote.email = roofleSubmission.email;
                aggregateQuote.phone = roofleSubmission.phone;
                aggregateQuote.market = roofleSubmission.market;

                aggregateQuote.timestamp = DateTime.UtcNow;

                aggregateQuote.numberOfStructures = roofleSubmission.numberOfStructures;
                aggregateQuote.numberOfIncludedStructures = roofleSubmission.numberOfIncludedStructures;
                aggregateQuote.totalSquareFeet = roofleSubmission.totalSquareFeet;

                aggregateQuote.repName = roofleSubmission.repName;
                aggregateQuote.repEmail = roofleSubmission.repEmail;
                aggregateQuote.leadId = roofleSubmission.leadId;

                aggregateQuote.products = roofleSubmission.products;
                aggregateQuote.structures = roofleSubmission.structures;

                aggregateQuote.mainRoofTotalSquareFeet = roofleSubmission.mainRoofTotalSquareFeet;
                aggregateQuote.totalInitialSquareFeet = roofleSubmission.totalInitialSquareFeet;
                aggregateQuote.sessionId = roofleSubmission.sessionId;

                var author = new ApplicationUser();
                author.Id = Guid.NewGuid().ToString();
                author.FirstName = "Web";
                author.LastName = "Api";

                Event _aggregateEvent = new Event();
                _aggregateEvent.Trade = aggregateQuote.applicationType;
                _aggregateEvent.DateTime = DateTime.UtcNow;
                _aggregateEvent.Name = "Aggregate Quote Created";
                _aggregateEvent.Details = "Aggregate Quote created & referred by: " + headerReferer;

                _aggregateEvent.Author = author;

                aggregateQuote.events.Add(_aggregateEvent);

                // Add Parent to queue
                newQuoteListQueue.Add(aggregateQuote);

                // Create Parent relationship
                var parentRelationship = new Relationship();
                parentRelationship.Author = author.FirstName + " " + author.LastName;
                parentRelationship._id = aggregateQuote._id;
                parentRelationship.ParentId = aggregateQuote._id;
                parentRelationship.Type = "Parent";
                parentRelationship.Name = aggregateQuote.applicationType;

                for (var i = 0; i < childQuotesToCreate; i++)
                {
                    Quote childQuote = new Quote(aggregateQuote);
                    childQuote._id = Guid.NewGuid().ToString();

                    childQuote.statusSubcategory = "spec-bidder";

                    // Will need to figure out how to set dynamically
                    childQuote.applicationType = utils.TitleCaseString("Roofing");

                    // Create Child relationship
                    var childRelationship = new Relationship();
                    childRelationship.Author = author.FirstName + " " + author.LastName;
                    childRelationship._id = childQuote._id;
                    childRelationship.ParentId = aggregateQuote._id;
                    childRelationship.Type = "Child";
                    childRelationship.Name = childQuote.applicationType;

                    // Add Child relationship to Parent
                    if(childRelationship.Type != "Parent")
                    {
                        aggregateQuote.relationships.Add(childRelationship);
                    }

                    // Create event details inside Parent
                    Event _parentEvent = new Event();
                    _parentEvent.Trade = childQuote.applicationType;
                    _parentEvent.DateTime = DateTime.UtcNow;
                    _parentEvent.Name = (i+1) + " Child (" + childQuote.applicationType + ") quote spawned";
                    _parentEvent.Details = (i + 1) + " Child (" + childQuote.applicationType + ") quote spawned from Parent Id (" + aggregateQuote._id + ")";
                    aggregateQuote.events.Add(_parentEvent);

                    // Create event details inside Child
                    Event _childEvent = new Event();

                    // Set child event trade
                    _childEvent.Trade = childQuote.applicationType;

                    _childEvent.DateTime = DateTime.UtcNow;
                    _childEvent.Name = (i + 1) + " New (" + childQuote.applicationType + ") quote Attached";
                    _childEvent.Details = (i + 1) + " New (" + childQuote.applicationType + ") quote attached to Parent Id (" + aggregateQuote._id + ")";
                    childQuote.events.Add(_childEvent);

                    // Add Parent relationship to Child
                    childQuote.relationships.Add(parentRelationship);

                    // Add Child to queue
                    newQuoteListQueue.Add(childQuote);
                }

                _quoteCollection = _mongoDBClient.GetDatabase(_mongoDBName).GetCollection<Quote>(_mongoDBCollectionName);

                foreach(Quote quote in newQuoteListQueue)
                {
                    _quoteCollection.InsertOne(quote);
                }

                resultMsg = "Added Aggregate QuoteId: " + aggregateQuote._id;
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

                        var currentDateTime = currentAttachment.DateTime;

                        currentAttachment.DateTime = currentAttachment.DateTime.ToLocalTime();

                        unSortedFiles.Add(currentAttachment);
                    }

                    for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
                    {
                        Note currentNote = sortedQuotes[i].notes[j];
                        currentNote.DateTime = currentNote.DateTime.ToLocalTime();

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
            return originalList.OrderByDescending(x => x.DateTime).ToList();
        }

        private List<Note> SortNotesByDateDesc(List<Note> originalList)
        {
            return originalList.OrderByDescending(x => x.DateTime).ToList();
        }
    }
}