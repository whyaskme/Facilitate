using Facilitate.Libraries.Models;
using Facilitate.Libraries.Services;
//using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Facilitate.Api.Controllers
{
    //[DisableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Utils utils;
        string _mongoDBCollectionName = "Quote";

        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<Quote> _quoteCollection;

        public ApplicationUser author = new ApplicationUser();

        public List<Quote> newQuoteListQueue = new List<Quote>();

        private int numQuotesToCreate = 0;

        string resultMsg = string.Empty;

        public TestController(DBService dBService, Utils utils)
        {
            _mongoDatabase = dBService.MongoDatabase;
            _quoteCollection = _mongoDatabase.GetCollection<Quote>(_mongoDBCollectionName);
            this.utils = utils;
        }


        // GET api/<TestController>/5
        [HttpGet("{numQuotesToCreate}")]
        public async Task<IActionResult> Get(string Trade, int numQuotesToCreate, int bidExpiresInDays, CancellationToken ct)
        {
            int numberOfChildrenToCreate = 0;

            if (Trade == null || Trade == "")
            {
                Trade = "Roofing";
            }

            Trade = utils.TitleCaseString(Trade);

            author = new ApplicationUser();
            author.Id = Guid.NewGuid().ToString();
            author.FirstName = "Roofle";
            author.LastName = "WebApi";

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

            string headerForwardedFor = "n/a";
            string headerReferer = "n/a";

            int childBidderQuotesToCreate = 1;

            double AvgPricePerSqFt = 4.50;
            double totalQuoteValue = 0;

            var requestHeaders = HttpContext.Request.Headers;

            List<String> nameGenders = new List<string>();
            nameGenders.Add("male");
            nameGenders.Add("female");

            Random rnd = new Random();
            int randomInt = 0;

            for (var h = 0; h < numQuotesToCreate; h++)
            {
                Quote aggregateQuote = new Quote();
                aggregateQuote.status = "Opportunity";

                rnd = new Random();
                randomInt = rnd.Next(0, 1);

                var firstName = await utils.GetRandomFirstNameAsync(nameGenders[randomInt], ct);
                var lastName = await utils.GetRandomLastNameAsync(ct);

                var randomStreetNumber = utils.GetRandomStreetNumber();
                var randomStreetName = await utils.GetRandomStreetNameAsync(ct);

                var randomState = await utils.GetRandomStateAsync(ct);
                var stateName = randomState[0];
                var stateAbbr = randomState[1];
                var stateId = randomState[2];

                var randomCity = await utils.GetRandomCityAsync(MongoDB.Bson.ObjectId.Parse(stateId), ct);

                string cityId = randomCity[0];
                string cityName = randomCity[1];
                string cityCountyId = randomCity[2];
                string cityTimeZoneId = randomCity[3];

                ZipCode tmpZipCode = await utils.GetRandomZipCodeAsync(MongoDB.Bson.ObjectId.Parse(cityId), ct);
                var randomZipCode = tmpZipCode.Zip.ToString();

                // Add Structure Info
                int totalSquareFeet = 0;
                int mainRoofTotalSquareFeet = 0;

                var repName = "";

                try
                {
                    foreach (var header in requestHeaders)
                    {
                        if (header.Key == "X-Forwarded-For")
                            headerForwardedFor = header.Value;

                        if (header.Key == "Referer")
                            headerReferer = header.Value;
                    }

                    // Create the parent Aggregate quote
                    aggregateQuote = new Quote();
                    aggregateQuote.status = "Opportunity";

                    aggregateQuote.Trade = utils.TitleCaseString("Aggregate");
                    aggregateQuote.TradeCategory = utils.TitleCaseString(Trade);

                    // Set Bidding properties
                    aggregateQuote.Bidder = author;
                    aggregateQuote.BidderType = "Parent";
                    aggregateQuote.BiddingExpires = DateTime.UtcNow.AddDays(bidExpiresInDays);

                    aggregateQuote.ipAddress = headerForwardedFor;
                    aggregateQuote.externalUrl = headerReferer;

                    aggregateQuote.firstName = firstName;
                    aggregateQuote.lastName = lastName;

                    aggregateQuote.email = aggregateQuote.firstName.ToLower() + "@" + aggregateQuote.lastName.ToLower() + ".com";
                    aggregateQuote.phone = "(" + utils.GetRandomAreaCode() + ") " + utils.GetRandomHomePhoneNumber();

                    randomInt = rnd.Next(5000, 9999);
                    aggregateQuote.leadId = randomInt;

                    randomInt = rnd.Next(1, 3);
                    aggregateQuote.numberOfStructures = randomInt;
                    aggregateQuote.numberOfIncludedStructures = aggregateQuote.numberOfStructures;

                    aggregateQuote.structures = null;
                    aggregateQuote.structures = new List<Structure>();

                    for (var j = 0; j < aggregateQuote.numberOfStructures; j++)
                    {
                        Structure structure = new Structure();
                        switch (j)
                        {
                            case 0:
                                structure.initialSquareFeet = rnd.Next(1000, 5000);
                                structure.isIncluded = true;
                                structure.name = "Main Roof";
                                structure.roofComplexity = "Complex";
                                structure.slope = "steep";
                                break;
                            case 1:
                                structure.initialSquareFeet = rnd.Next(500, 1500);
                                structure.isIncluded = true;
                                structure.name = "Garage Roof";
                                structure.roofComplexity = "Simple";
                                structure.slope = "medium";
                                break;
                            default:
                                structure.initialSquareFeet = rnd.Next(250, 1000);
                                structure.isIncluded = false;
                                structure.name = "Misc Roof";
                                structure.roofComplexity = "Compound";
                                structure.slope = "shallow";
                                break;
                        }

                        structure.squareFeet = structure.initialSquareFeet;

                        aggregateQuote.structures.Add(structure);

                        totalSquareFeet += structure.initialSquareFeet;
                    }

                    aggregateQuote.totalSquareFeet = totalSquareFeet;
                    aggregateQuote.totalInitialSquareFeet = aggregateQuote.totalSquareFeet;

                    aggregateQuote.mainRoofTotalSquareFeet = totalSquareFeet;

                    aggregateQuote.sessionId = Guid.NewGuid().ToString();

                    aggregateQuote.totalQuote = (totalSquareFeet * AvgPricePerSqFt);

                    aggregateQuote.address = randomStreetNumber + " " + randomStreetName + ", " + cityName + ", " + stateAbbr + " " + randomZipCode;
                    aggregateQuote.fullAddress = aggregateQuote.address;
                    aggregateQuote.street = randomStreetNumber + " " + randomStreetName;
                    aggregateQuote.city = cityName;

                    aggregateQuote.state = stateAbbr;
                    aggregateQuote.zip = randomZipCode;

                    aggregateQuote.market = aggregateQuote.city + ", " + aggregateQuote.state;

                    aggregateQuote.timestamp = DateTime.UtcNow;

                    randomInt = rnd.Next(0, 1);
                    repName = await utils.GetRandomFirstNameAsync(nameGenders[randomInt], ct) + " " + await utils.GetRandomLastNameAsync(ct);

                    aggregateQuote.repName = repName;
                    aggregateQuote.repEmail = repName.Replace(" ", ".").ToLower() + "@facilitate.org";

                    randomInt = rnd.Next(5000, 9999);
                    aggregateQuote.leadId = randomInt;

                    CreateParentSpawnedEvent(aggregateQuote);

                    // Create Children
                    for (var i = 0; i <= numberOfChildrenToCreate; i++)
                    {
                        try
                        {
                            // Create Child Quote
                            Quote childQuote = new Quote();
                            childQuote.status = aggregateQuote.status;

                            childQuote.repName = aggregateQuote.repName;
                            childQuote.repEmail = aggregateQuote.repEmail;
                            childQuote.leadId = aggregateQuote.leadId;

                            childQuote.totalQuote = 0;

                            childQuote.status = "Opportunity";
                            childQuote.Trade = Trade;
                            childQuote.TradeCategory = ""; // Trade;

                            Event createdEvent = new Event();
                            createdEvent.Author = author;
                            createdEvent.Trade = childQuote.Trade;
                            createdEvent.Name = "Child (" + childQuote.Trade + ") quote";
                            createdEvent.Details = createdEvent.Name + " Spawned from Aggregate Id (" + createdEvent._id + ")";

                            childQuote.events.Add(createdEvent);

                            childQuote.Bidder = author;
                            childQuote.BiddingExpires = aggregateQuote.BiddingExpires;

                            switch (i)
                            {
                                case 0:
                                    childQuote.BidderType = "Default";
                                    childQuote.totalQuote = aggregateQuote.totalQuote;
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

                            childQuote.ipAddress = aggregateQuote.ipAddress;
                            childQuote.externalUrl = aggregateQuote.externalUrl;

                            childQuote.mainRoofTotalSquareFeet = aggregateQuote.mainRoofTotalSquareFeet;

                            childQuote.totalSquareFeet = aggregateQuote.totalSquareFeet;
                            childQuote.totalInitialSquareFeet = aggregateQuote.totalInitialSquareFeet;

                            childQuote.firstName = aggregateQuote.firstName;
                            childQuote.lastName = aggregateQuote.lastName;
                            childQuote.email = aggregateQuote.email;
                            childQuote.phone = aggregateQuote.phone;
                            childQuote.address = aggregateQuote.address;
                            childQuote.fullAddress = aggregateQuote.fullAddress;
                            childQuote.street = aggregateQuote.street;
                            childQuote.city = aggregateQuote.city;
                            childQuote.state = aggregateQuote.state;
                            childQuote.zip = aggregateQuote.zip;
                            childQuote.market = aggregateQuote.market;

                            // Create Aggregate relationship to Child
                            var parentRelationship = new Relationship();
                            parentRelationship.Author = author.FirstName + " " + author.LastName;
                            parentRelationship._id = aggregateQuote._id;
                            parentRelationship.ParentId = aggregateQuote._id;
                            parentRelationship.Type = "Parent";
                            parentRelationship.Name = aggregateQuote.Trade;
                            childQuote.relationships.Add(parentRelationship);

                            Event parentRelationshipEvent = new Event();
                            parentRelationshipEvent.Author = author;
                            parentRelationshipEvent.Trade = childQuote.Trade;
                            parentRelationshipEvent.Name = "Child (" + childQuote.Trade + ") relationship Linked to Parent";
                            parentRelationshipEvent.Details = parentRelationshipEvent.Name + " Aggregate Id (" + aggregateQuote._id + ")";
                            childQuote.events.Add(parentRelationshipEvent);

                            // Add to queue for sibling relationship processing
                            newQuoteListQueue.Add(childQuote);

                            // Save Child relationship to Aggregate
                            var childRelationship = new Relationship();
                            childRelationship.Author = author.FirstName + " " + author.LastName;
                            childRelationship._id = childQuote._id;
                            childRelationship.ParentId = aggregateQuote._id;
                            childRelationship.Type = "Child";
                            childRelationship.Name = childQuote.Trade;

                            aggregateQuote.relationships.Add(childRelationship);

                            // Insert Child
                            await _quoteCollection.InsertOneAsync(childQuote, null, ct);

                            childQuote = null;
                        }
                        catch (Exception ex)
                        {
                            resultMsg = ex.Message;
                        }
                        finally
                        {
                        }
                    }

                    resultMsg = numQuotesToCreate + " Quotes added";

                    //CreateSiblingRelationships(aggregateQuote, newQuoteListQueue);

                    // Reset value since the Default quote contains it
                    aggregateQuote.totalQuote = 0;
                    await _quoteCollection.InsertOneAsync(aggregateQuote, null, ct);
                }
                catch (Exception ex)
                {
                    resultMsg = ex.Message;
                    return BadRequest(resultMsg);
                }
                finally
                {

                }
            }

            return Ok(resultMsg);
        }

        private async Task CreateChildQuotesAsync(Quote aggregateQuote, CancellationToken ct = default)
        {
            try
            {
                foreach(Relationship relationship in aggregateQuote.relationships)
                {
                    // Create Child Quote
                    Quote childQuote = new Quote();
                    childQuote.status = "Opportunity";
                    childQuote.Trade = aggregateQuote.Trade;

                    Event createdEvent = new Event();
                    createdEvent.Author = author;
                    createdEvent.Trade = childQuote.Trade;
                    createdEvent.Name = "Child (" + childQuote.Trade + ") quote";
                    createdEvent.Details = createdEvent.Name + " Spawned from Aggregate Id (" + createdEvent._id + ")";

                    childQuote.events.Add(createdEvent);

                    childQuote.Bidder = author;
                    childQuote.BidderType = "Spec";
                    childQuote.totalQuote = 0;

                    childQuote.BiddingExpires = aggregateQuote.BiddingExpires;

                    childQuote.ipAddress = aggregateQuote.ipAddress;
                    childQuote.externalUrl = aggregateQuote.externalUrl;

                    childQuote.mainRoofTotalSquareFeet = aggregateQuote.mainRoofTotalSquareFeet;

                    childQuote.totalSquareFeet = aggregateQuote.totalSquareFeet;
                    childQuote.totalInitialSquareFeet = aggregateQuote.totalInitialSquareFeet;

                    childQuote.firstName = aggregateQuote.firstName;
                    childQuote.lastName = aggregateQuote.lastName;
                    childQuote.email = aggregateQuote.email;
                    childQuote.phone = aggregateQuote.phone;
                    childQuote.address = aggregateQuote.address;
                    childQuote.fullAddress = aggregateQuote.fullAddress;
                    childQuote.street = aggregateQuote.street;
                    childQuote.city = aggregateQuote.city;
                    childQuote.state = aggregateQuote.state;
                    childQuote.zip = aggregateQuote.zip;
                    childQuote.market = aggregateQuote.market;

                    // Create Aggregate relationship to Child
                    var parentRelationship = new Relationship();
                    parentRelationship.Author = author.FirstName + " " + author.LastName;
                    parentRelationship._id = aggregateQuote._id;
                    parentRelationship.ParentId = aggregateQuote._id;
                    parentRelationship.Type = "Parent";
                    parentRelationship.Name = aggregateQuote.Trade;
                    childQuote.relationships.Add(parentRelationship);

                    Event parentRelationshipEvent = new Event();
                    parentRelationshipEvent.Author = author;
                    parentRelationshipEvent.Trade = childQuote.Trade;
                    parentRelationshipEvent.Name = "Child (" + childQuote.Trade + ") relationship Linked to Parent";
                    parentRelationshipEvent.Details = parentRelationshipEvent.Name + " Aggregate Id (" + aggregateQuote._id + ")";
                    childQuote.events.Add(parentRelationshipEvent);

                    // Save Child
                    await _quoteCollection.InsertOneAsync(childQuote, null, ct);

                    // Add to queue for sibling relationship processing
                    newQuoteListQueue.Add(childQuote);

                    // Save Child relationship to Aggregate
                    var childRelationship = new Relationship();
                    childRelationship.Author = author.FirstName + " " + author.LastName;
                    childRelationship._id = childQuote._id;
                    childRelationship.ParentId = aggregateQuote._id;
                    childRelationship.Type = "Child";
                    childRelationship.Name = childQuote.Trade;

                    aggregateQuote.relationships.Add(childRelationship);

                    var filter = Builders<Quote>.Filter.Eq(x => x._id, aggregateQuote._id);
                    var result = await _quoteCollection.ReplaceOneAsync(filter, aggregateQuote, new ReplaceOptions() { IsUpsert = true }, ct);

                    childQuote = null;
                }
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {
            }
        }

        private void CreateParentSpawnedEvent(Quote aggregateQuote)
        {
            Event _event = new Event();
            _event.Author = author;
            _event.Trade = aggregateQuote.Trade;
            _event.Name = aggregateQuote.Trade + " quote Created";
            _event.Details = _event.Name;

            // Add event to child quote
            aggregateQuote.events.Add(_event);
        }

        private void CreateChildSpawnedEvent(Quote aggregateQuote, Quote childQuote)
        {
            // Child event
            Event childEvent = new Event();
            childEvent.Author = author;
            childEvent.Trade = childQuote.Trade;
            childEvent.Name = "Child (" + childQuote.Trade + ") quote Spawned";
            childEvent.Details = childEvent.Name + " for parent Aggregate Id (" + aggregateQuote._id + ")";

            // Add event to child quote
            childQuote.events.Add(childEvent);
        }

        private void CreateChildLinkedToParentEvent(Quote aggregateQuote, Quote childQuote)
        {
            Event childEvent = new Event();
            childEvent.Author = author;
            childEvent.Trade = childQuote.Trade;
            childEvent.Name = "Child (" + childQuote.Trade + ") quote Linked";
            childEvent.Details = childEvent.Name + " to parent Aggregate Id (" + aggregateQuote._id + ")";

            // Add event to child quote
            childQuote.events.Add(childEvent);
        }

        private async Task CreateSiblingRelationshipsAsync(Quote aggregateQuote, List<Quote> newQuoteListQueue, CancellationToken ct = default)
        {
            var author = new ApplicationUser();
            author.Id = Guid.NewGuid().ToString();
            author.FirstName = "Roofle";
            author.LastName = "WebApi";

            var relatedQuoteId = "";
            var relationshipType = "";

            try
            {
                foreach (Relationship _relationship in aggregateQuote.relationships)
                {
                    if (_relationship.Type == "Child")
                    {
                        relatedQuoteId = _relationship._id;

                        Quote relatedQuoteUpdate = await _quoteCollection.Find(x => x._id == relatedQuoteId).FirstOrDefaultAsync(ct);

                        foreach (Quote newQuote in newQuoteListQueue)
                        {
                            if (_relationship.ParentId != newQuote._id)
                            {
                                Quote quoteToUpdate = await _quoteCollection.Find(x => x._id == newQuote._id).FirstOrDefaultAsync(ct);

                                // Create Sibling relationship
                                var siblingRelationship = new Relationship();
                                siblingRelationship.Author = author.FirstName + " " + author.LastName;
                                siblingRelationship._id = newQuote._id;

                                siblingRelationship.ParentId = relatedQuoteId;

                                siblingRelationship.Type = "Sibling";
                                siblingRelationship.Name = newQuote.Trade;

                                quoteToUpdate.relationships.Add(siblingRelationship);


                                Event siblingEvent = new Event();
                                siblingEvent.Author = author;

                                siblingEvent.Trade = newQuote.Trade;

                                siblingEvent.Name = "Sibling (" + newQuote.Trade + ") quote Linked";
                                siblingEvent.Details = siblingEvent.Name + " to (" + newQuote.Trade + ") Sibling Id (" + siblingEvent._id + ")";

                                // Add event to child quote
                                quoteToUpdate.events.Add(siblingEvent);

                                // Save Child
                                var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteToUpdate._id);
                                var result = await _quoteCollection.ReplaceOneAsync(filter, quoteToUpdate, new ReplaceOptions() { IsUpsert = true }, ct);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {
                var tmpVal = "Fianlly!";
            }
        }

    }
}
