﻿using Facilitate.Libraries.Models;
using Facilitate.Libraries.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
//using Microsoft.AspNetCore.Cors;

namespace Facilitate.Api.Controllers
{
    //[DisableCors]
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

        public List<Quote> newQuoteListQueue = new List<Quote>();

        public ApplicationUser author = new ApplicationUser();

        [ProducesResponseType<String>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuoteRoofleSubmission roofleSubmission, CancellationToken ct)
        {
            Quote aggregateQuote = new Quote();

            author = new ApplicationUser();
            author.Id = Guid.NewGuid().ToString();
            author.FirstName = "Roofle";
            author.LastName = "Api";

            author.PhoneNumber = "555-555-5555";
            author.PhoneNumberConfirmed = true;

            author.Address1 = "123 Main St";
            author.Address2 = "";
            author.City = "Austin";
            author.State = "TX";
            author.Zip = "78753";

            author.Email = "admin@facilitate.org";
            author.EmailConfirmed = true;
            
            author.UserName = "RoofleApi";
            author.NormalizedUserName = "ROOFLEAPI";
            author.NormalizedEmail = author.Email;
            author.LockoutEnabled = false;
            author.LockoutEnd = null;
            author.ConcurrencyStamp = Guid.NewGuid().ToString();
            author.SecurityStamp = Guid.NewGuid().ToString();
            author.PasswordHash = "";

            try
            {
                string headerForwardedFor = "n/a";
                string headerReferer = "n/a";

                int childBidderQuotesToCreate = 0;
                int BiddingExpiresInDays = 7;

                double totalQuoteValue = 0;

                var requestHeaders = HttpContext.Request.Headers;
                foreach (var header in requestHeaders)
                {
                    if (header.Key == "X-Forwarded-For")
                        headerForwardedFor = header.Value;

                    if (header.Key == "Referer")
                        headerReferer = header.Value;
                }

                aggregateQuote.status = "Opportunity";

                aggregateQuote.Trade = utils.TitleCaseString("Aggregate");
                aggregateQuote.TradeCategory = "Roofing";

                // Set Bidding properties
                aggregateQuote.Bidder = author;
                aggregateQuote.BiddingExpires = DateTime.UtcNow.AddDays(BiddingExpiresInDays);
                aggregateQuote.BidderType = utils.TitleCaseString("Aggregate");

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

                var stateAbbreviation = await utils.GetStateAbbrByNameAsync(roofleSubmission.state, ct);

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

                Event _event = new Event();
                _event.Author = author;
                _event.Trade = aggregateQuote.Trade;
                _event.Name = aggregateQuote.Trade + " quote Created";
                _event.Details = _event.Name;

                // Add event to child quote
                aggregateQuote.events.Add(_event);

                for (var i = 0; i <= childBidderQuotesToCreate; i++)
                {
                    Quote childQuote = new Quote();
                    childQuote.status = aggregateQuote.status;

                    childQuote.repName = aggregateQuote.repName;
                    childQuote.repEmail = aggregateQuote.repEmail;
                    childQuote.leadId = aggregateQuote.leadId;

                    childQuote.totalQuote = 0;

                    switch (i)
                    {
                        case 0:
                            childQuote.BidderType = "Default";
                            childQuote.totalQuote = totalQuoteValue;
                            childQuote.structures = aggregateQuote.structures;
                            childQuote.numberOfStructures = aggregateQuote.numberOfStructures;
                            childQuote.numberOfIncludedStructures = aggregateQuote.numberOfIncludedStructures;
                            childQuote.totalSquareFeet = aggregateQuote.totalSquareFeet;
                            childQuote.totalInitialSquareFeet = aggregateQuote.totalInitialSquareFeet;
                            childQuote.mainRoofTotalSquareFeet = aggregateQuote.mainRoofTotalSquareFeet;
                            childQuote.sessionId = aggregateQuote.sessionId;
                            break;
                        case 1:
                            childQuote.BidderType = "Spec";
                            childQuote.totalQuote = 0;
                            break;
                        default:
                            childQuote.BidderType = "Open";
                            childQuote.totalQuote = 0;
                            break;
                    }

                    // Will need to figure out how to set dynamically
                    childQuote.Trade = utils.TitleCaseString("Roofing");
                    childQuote.TradeCategory = ""; // utils.TitleCaseString("Child");

                    // Set Bidding properties
                    childQuote.Bidder = author;
                    childQuote.BiddingExpires = DateTime.UtcNow.AddDays(BiddingExpiresInDays);

                    // Begin copy data
                    childQuote.ipAddress = headerForwardedFor;
                    childQuote.externalUrl = headerReferer;

                    if (aggregateQuote.products[0].priceInfo.total != null)
                    {
                        totalQuoteValue = aggregateQuote.products[0].priceInfo.total;
                    }

                    childQuote.address = aggregateQuote.address;
                    childQuote.fullAddress = aggregateQuote.fullAddress;
                    childQuote.street = aggregateQuote.street;
                    childQuote.city = aggregateQuote.city;
                    childQuote.state = aggregateQuote.state;
                    childQuote.zip = aggregateQuote.zip;

                    childQuote.firstName = aggregateQuote.firstName;
                    childQuote.lastName = aggregateQuote.lastName;
                    childQuote.email = aggregateQuote.email;
                    childQuote.phone = aggregateQuote.phone;
                    childQuote.market = aggregateQuote.market;

                    childQuote.timestamp = DateTime.UtcNow;

                    childQuote.numberOfStructures = aggregateQuote.numberOfStructures;
                    childQuote.numberOfIncludedStructures = aggregateQuote.numberOfIncludedStructures;
                    childQuote.totalSquareFeet = aggregateQuote.totalSquareFeet;

                    childQuote.repName = aggregateQuote.repName;
                    childQuote.repEmail = aggregateQuote.repEmail;
                    childQuote.leadId = aggregateQuote.leadId;

                    childQuote.products = aggregateQuote.products;
                    childQuote.structures = aggregateQuote.structures;

                    childQuote.mainRoofTotalSquareFeet = aggregateQuote.mainRoofTotalSquareFeet;
                    childQuote.totalInitialSquareFeet = aggregateQuote.totalInitialSquareFeet;
                    childQuote.sessionId = aggregateQuote.sessionId;
                    // End copy data

                    // Create Aggregate relationship to Child
                    var parentRelationship = new Relationship();
                    parentRelationship.Author = author.FirstName + " " + author.LastName;
                    parentRelationship._id = aggregateQuote._id;
                    parentRelationship.ParentId = aggregateQuote._id;
                    parentRelationship.Type = "Parent";
                    parentRelationship.Name = aggregateQuote.Trade;

                    childQuote.relationships.Add(parentRelationship);

                    // Create Child relationship to Aggregate
                    var childRelationship = new Relationship();
                    childRelationship.Author = author.FirstName + " " + author.LastName;
                    childRelationship._id = childQuote._id;
                    childRelationship.ParentId = aggregateQuote._id;
                    childRelationship.Type = "Child";
                    childRelationship.Name = childQuote.Trade;

                    // Add relationships
                    aggregateQuote.relationships.Add(childRelationship);

                    // Add Events
                    // Child Linked event
                    Event childLinkedEvent = new Event();
                    childLinkedEvent.Author = author;
                    childLinkedEvent.Trade = childQuote.Trade;
                    childLinkedEvent.Name = "Child (" + childQuote.Trade + ") quote Linked";
                    childLinkedEvent.Details = childLinkedEvent.Name + " to Aggregate Id (" + aggregateQuote._id + ")";

                    // Add event to child quote
                    childQuote.events.Add(childLinkedEvent);

                    // Child Spawned event
                    Event childSpawnedEvent = new Event();
                    childSpawnedEvent.Author = author;
                    childSpawnedEvent.Trade = childQuote.Trade;
                    childSpawnedEvent.Name = "Child (" + childQuote.Trade + ") quote Spawned";
                    childSpawnedEvent.Details = childSpawnedEvent.Name + " for Aggregate Id (" + aggregateQuote._id + ")";

                    // Add event to child quote
                    childQuote.events.Add(childSpawnedEvent);

                    // Insert Child
                    childQuote.relationships = childQuote.relationships.Distinct().ToList();
                    await _quoteCollection.InsertOneAsync(childQuote, null, ct);

                    // Add to queue for sibling relationship processing
                    newQuoteListQueue.Add(childQuote);
                }

                // Insert Aggregate
                aggregateQuote.relationships = aggregateQuote.relationships.Distinct().ToList();
                await _quoteCollection.InsertOneAsync(aggregateQuote, null, ct);

                //CreateSiblingRelationships(aggregateQuote, newQuoteListQueue);

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
            return Ok(aggregateQuote);
        }

        private async Task CreateSiblingRelationshipsAsync(Quote aggregateQuote, List<Quote> newQuoteListQueue, CancellationToken ct = default)
        {
            var author = new ApplicationUser();
            author.Id = Guid.NewGuid().ToString();
            author.FirstName = "Web";
            author.LastName = "Api";

            List<Quote> referenceQuoteList = new List<Quote>(newQuoteListQueue);

            try
            {
                foreach (Quote newQuote in newQuoteListQueue)
                {
                    foreach(Quote relatedSiblinigQuote in referenceQuoteList)
                    {
                        //if (newQuote._id == relatedSiblinigQuote._id)
                        //{
                        //    continue;
                        //}

                        // Create Sibling relationship
                        var siblingRelationship = new Relationship();
                        siblingRelationship.Author = author.FirstName + " " + author.LastName;
                        siblingRelationship._id = newQuote._id;
                        siblingRelationship.ParentId = relatedSiblinigQuote._id;
                        siblingRelationship.Type = "Sibling";
                        siblingRelationship.Name = newQuote.Trade;

                        newQuote.relationships = newQuote.relationships.Distinct().ToList();

                        newQuote.relationships.Add(siblingRelationship);

                        Event siblingEvent = new Event();
                        siblingEvent.Author = author;
                        siblingEvent.Trade = newQuote.Trade;
                        siblingEvent.Name = "Sibling (" + newQuote.Trade + ") quote Linked";
                        siblingEvent.Details = siblingEvent.Name + " to Sibling Id (" + siblingEvent._id + ")";

                        // Add event to child quote
                        newQuote.events.Add(siblingEvent);

                        var filter = Builders<Quote>.Filter.Eq(x => x._id, newQuote._id);
                        var result = await _quoteCollection.ReplaceOneAsync(filter, newQuote, new ReplaceOptions() { IsUpsert = true }, ct);
                    }

                    if (newQuote.Trade != "Aggregate")
                    {
                        // Create Sibling relationship
                        var siblingRelationship = new Relationship();
                        siblingRelationship.Author = author.FirstName + " " + author.LastName;
                        siblingRelationship._id = newQuote._id;
                        siblingRelationship.ParentId = aggregateQuote._id;
                        siblingRelationship.Type = "Sibling";
                        siblingRelationship.Name = newQuote.Trade;

                        newQuote.relationships = newQuote.relationships.Distinct().ToList();

                        newQuote.relationships.Add(siblingRelationship);

                        Event siblingEvent = new Event();
                        siblingEvent.Author = author;
                        siblingEvent.Trade = newQuote.Trade;
                        siblingEvent.Name = "Sibling (" + newQuote.Trade + ") quote Linked";
                        siblingEvent.Details = siblingEvent.Name + " to Sibling Id (" + siblingEvent._id + ")";

                        // Add event to child quote
                        newQuote.events.Add(siblingEvent);

                        var filter = Builders<Quote>.Filter.Eq(x => x._id, newQuote._id);
                        var result = await _quoteCollection.ReplaceOneAsync(filter, newQuote, new ReplaceOptions() { IsUpsert = true }, ct);
                    }
                }

                //childQuote.relationships = childQuote.relationships.Distinct().ToList();

                //var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteId);

                //var result = await _quoteCollection.ReplaceOneAsync(filter, quoteToUpdate, new ReplaceOptions() { IsUpsert = true }, ct);
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }

            foreach (Relationship _relationship in aggregateQuote.relationships)
            {

            }
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

                //for (var i = 0; i < sortedQuotes.Count; i++)
                //{
                //    unSortedFiles.Clear();
                //    unSortedNotes.Clear();
                //    unSortedEvents.Clear();

                //    for (var j = 0; j < sortedQuotes[i].attachments.Count; j++)
                //    {
                //        Attachment currentAttachment = sortedQuotes[i].attachments[j];

                //        var currentDateTime = currentAttachment.DateTime;

                //        currentAttachment.DateTime = currentAttachment.DateTime.ToLocalTime();

                //        unSortedFiles.Add(currentAttachment);
                //    }

                //    for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
                //    {
                //        Note currentNote = sortedQuotes[i].notes[j];
                //        currentNote.DateTime = currentNote.DateTime.ToLocalTime();

                //        unSortedNotes.Add(currentNote);
                //    }

                //    for (var j = 0; j < sortedQuotes[i].events.Count; j++)
                //    {
                //        Event currentEvent = sortedQuotes[i].events[j];
                //        currentEvent.DateTime = currentEvent.DateTime.ToLocalTime();

                //        unSortedEvents.Add(currentEvent);
                //    }

                //    sortedQuotes[i].attachments = SortFilesByDateDesc(unSortedFiles);
                //    sortedQuotes[i].notes = SortNotesByDateDesc(unSortedNotes);
                //    sortedQuotes[i].events = SortEventsByDateDesc(unSortedEvents);
                //}
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