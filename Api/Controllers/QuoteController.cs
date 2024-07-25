using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

using System.Net.Http;
using System.Threading;
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

        CancellationToken _cancellationToken;

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

                int childBidderQuotesToCreate = 5;
                int biddingExpiresInDays = 7;

                double totalQuoteValue = 0;

                var requestHeaders = HttpContext.Request.Headers;
                foreach (var header in requestHeaders)
                {
                    if (header.Key == "X-Forwarded-For")
                        headerForwardedFor = header.Value;

                    if (header.Key == "Referer")
                        headerReferer = header.Value;
                }

                Quote aggregateQuote = new Quote();
                aggregateQuote.applicationType = utils.TitleCaseString("Aggregate");

                // Set expiration
                aggregateQuote.biddingExpires = DateTime.UtcNow.AddDays(biddingExpiresInDays);

                aggregateQuote.ipAddress = headerForwardedFor;
                aggregateQuote.externalUrl = headerReferer;

                if (roofleSubmission.products[0].priceInfo.total != null)
                {
                    totalQuoteValue = roofleSubmission.products[0].priceInfo.total;
                }

                aggregateQuote.totalQuote = 0;

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

                for (var i = 0; i < childBidderQuotesToCreate; i++)
                {
                    Quote childQuote = new Quote();

                    childQuote.statusSubcategory = "bidder";
                    childQuote.totalQuote = 0;

                    if (i == 0)
                    {
                        childQuote.statusSubcategory = "spec-bidder";
                        childQuote.totalQuote = totalQuoteValue;
                    }

                    // Will need to figure out how to set dynamically
                    childQuote.applicationType = utils.TitleCaseString("Roofing");

                    // Set expiration
                    childQuote.biddingExpires = DateTime.UtcNow.AddDays(biddingExpiresInDays);

                    // Begin copy data
                    childQuote.ipAddress = headerForwardedFor;
                    childQuote.externalUrl = headerReferer;

                    if (roofleSubmission.products[0].priceInfo.total != null)
                    {
                        totalQuoteValue = roofleSubmission.products[0].priceInfo.total;
                    }

                    childQuote.totalQuote = 0;

                    childQuote.address = roofleSubmission.address;
                    childQuote.fullAddress = roofleSubmission.fullAddress;
                    childQuote.street = roofleSubmission.street;
                    childQuote.city = roofleSubmission.city;

                    var stateAbbr = utils.GetStateAbbrByName(roofleSubmission.state);

                    childQuote.state = stateAbbr;
                    childQuote.zip = roofleSubmission.zip;

                    childQuote.firstName = roofleSubmission.firstName;
                    childQuote.lastName = roofleSubmission.lastName;
                    childQuote.email = roofleSubmission.email;
                    childQuote.phone = roofleSubmission.phone;
                    childQuote.market = roofleSubmission.market;

                    childQuote.timestamp = DateTime.UtcNow;

                    childQuote.numberOfStructures = roofleSubmission.numberOfStructures;
                    childQuote.numberOfIncludedStructures = roofleSubmission.numberOfIncludedStructures;
                    childQuote.totalSquareFeet = roofleSubmission.totalSquareFeet;

                    childQuote.repName = roofleSubmission.repName;
                    childQuote.repEmail = roofleSubmission.repEmail;
                    childQuote.leadId = roofleSubmission.leadId;

                    childQuote.products = roofleSubmission.products;
                    childQuote.structures = roofleSubmission.structures;

                    childQuote.mainRoofTotalSquareFeet = roofleSubmission.mainRoofTotalSquareFeet;
                    childQuote.totalInitialSquareFeet = roofleSubmission.totalInitialSquareFeet;
                    childQuote.sessionId = roofleSubmission.sessionId;
                    // End copy data

                    var author = new ApplicationUser();
                    author.Id = Guid.NewGuid().ToString();
                    author.FirstName = "Web";
                    author.LastName = "Api";

                    // Create Child relationship to Aggregate
                    var childRelationship = new Relationship();
                    childRelationship.Author = author.FirstName + " " + author.LastName;
                    childRelationship._id = childQuote._id;
                    childRelationship.ParentId = aggregateQuote._id;
                    childRelationship.Type = "Child";
                    childRelationship.Name = childQuote.applicationType;

                    aggregateQuote.relationships.Add(childRelationship);

                    // Create Aggregate relationship to Child
                    var parentRelationship = new Relationship();
                    parentRelationship.Author = author.FirstName + " " + author.LastName;
                    parentRelationship._id = aggregateQuote._id;
                    parentRelationship.ParentId = aggregateQuote._id;
                    parentRelationship.Type = "Parent";
                    parentRelationship.Name = aggregateQuote.applicationType;

                    // Add Parent to Child
                    childQuote.relationships.Add(parentRelationship);

                    // Add Child to Parent
                    aggregateQuote.relationships.Add(childRelationship);

                    // Insert Child
                    childQuote.relationships = childQuote.relationships.Distinct().ToList();
                    _quoteCollection.InsertOne(childQuote);

                    // Add to queue for sibling relationship processing
                    newQuoteListQueue.Add(childQuote);
                }

                // Insert Aggregate
                aggregateQuote.relationships = aggregateQuote.relationships.Distinct().ToList();
                _quoteCollection.InsertOne(aggregateQuote);

                CreateSiblingRelationships(aggregateQuote, newQuoteListQueue);

                //SaveQuotes(aggregateQuote, newQuoteListQueue);

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

        private void CreateSiblingRelationships(Quote aggregateQuote, List<Quote> newQuoteListQueue)
        {
            var author = new ApplicationUser();
            author.Id = Guid.NewGuid().ToString();
            author.FirstName = "Web";
            author.LastName = "Api";

            var quoteId = "";

            foreach (Relationship _relationship in aggregateQuote.relationships)
            {
                try
                {
                    quoteId = _relationship._id;

                    Quote quoteToUpdate = _quoteCollection.Find(x => x._id == quoteId).FirstOrDefault();

                    foreach (Quote newQuote in newQuoteListQueue)
                    {
                        // Create Sibling relationship
                        var siblingRelationship = new Relationship();
                        siblingRelationship.Author = author.FirstName + " " + author.LastName;
                        siblingRelationship._id = newQuote._id;
                        siblingRelationship.ParentId = quoteId;
                        siblingRelationship.Type = "Sibling";
                        siblingRelationship.Name = newQuote.applicationType;

                        quoteToUpdate.relationships.Add(siblingRelationship);
                    }

                    var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteId);

                    var result = _quoteCollection.ReplaceOne(filter, quoteToUpdate, new UpdateOptions() { IsUpsert = true }, _cancellationToken);

                    var tmpVal = "";
                }
                catch (Exception ex)
                {
                    resultMsg = ex.Message;
                }
                finally
                {

                }
            }
        }

        private void SaveQuotes(Quote aggregateQuote, List<Quote> newQuoteListQueue)
        {
            //_quoteCollection.InsertOne(aggregateQuote);

            foreach (Quote newQuote in newQuoteListQueue)
            {
                // Add children
                _quoteCollection.InsertOne(newQuote);
            }
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