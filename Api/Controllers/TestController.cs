﻿using DevExpress.DocumentServices.ServiceModel.DataContracts;
using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http.Cors;

namespace Facilitate.Api.Controllers
{
    [DisableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {
            _mongoDBClient = new MongoClient(_mongoDBConnectionString);
            _mongoDBCollection = _mongoDBClient.GetDatabase(_mongoDBName).GetCollection<Quote>(_mongoDBCollectionName);
        }

        Utils utils = new Utils();

        string _mongoDBName = "Facilitate";
        string _mongoDBCollectionName = "Quote";

        //string _mongoDBConnectionString = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate;safe=true;maxpoolsize=200";
        string _mongoDBConnectionString = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate;safe=true;maxpoolsize=200";

        IMongoClient _mongoDBClient;

        IMongoCollection<Quote> _mongoDBCollection;
        private CancellationToken _cancellationToken;

        IMongoCollection<Quote> _quoteCollection;

        string resultMsg = string.Empty;

        public ApplicationUser author = new ApplicationUser();

        public List<Quote> newQuoteListQueue = new List<Quote>();

        private int numQuotesToCreate = 0;

        // GET api/<TestController>/5
        [HttpGet("{numQuotesToCreate}")]
        public IActionResult Get(string Trade, int numQuotesToCreate, int bidExpiresInDays)
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

                rnd = new Random();
                randomInt = rnd.Next(0, 1);

                var firstName = utils.GetRandomFirstName(nameGenders[randomInt]);
                var lastName = utils.GetRandomLastName();

                var randomStreetNumber = utils.GetRandomStreetNumber();
                var randomStreetName = utils.GetRandomStreetName();

                var randomState = utils.GetRandomState();
                var stateName = randomState[0];
                var stateAbbr = randomState[1];
                var stateId = randomState[2];

                var randomCity = utils.GetRandomCity(MongoDB.Bson.ObjectId.Parse(stateId));

                string cityId = randomCity[0];
                string cityName = randomCity[1];
                string cityCountyId = randomCity[2];
                string cityTimeZoneId = randomCity[3];

                ZipCode tmpZipCode = utils.GetRandomZipCode(MongoDB.Bson.ObjectId.Parse(cityId));
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

                    _quoteCollection = _mongoDBClient.GetDatabase(_mongoDBName).GetCollection<Quote>(_mongoDBCollectionName);

                    // Create the parent Aggregate quote
                    aggregateQuote = new Quote();
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
                    repName = utils.GetRandomFirstName(nameGenders[randomInt]) + " " + utils.GetRandomLastName();

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

                            childQuote.repName = aggregateQuote.repName;
                            childQuote.repEmail = aggregateQuote.repEmail;
                            childQuote.leadId = aggregateQuote.leadId;

                            childQuote.totalQuote = 0;

                            childQuote.status = "New";
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
                            _quoteCollection.InsertOne(childQuote);

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
                    _quoteCollection.InsertOne(aggregateQuote);
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

        private void CreateChildQuotes(Quote aggregateQuote)
        {
            try
            {
                foreach(Relationship relationship in aggregateQuote.relationships)
                {
                    // Create Child Quote
                    Quote childQuote = new Quote();
                    childQuote.status = "New";
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
                    _quoteCollection.InsertOne(childQuote);

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
                    var result = _quoteCollection.ReplaceOne(filter, aggregateQuote, new UpdateOptions() { IsUpsert = true }, _cancellationToken);

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

        private void CreateSiblingRelationships(Quote aggregateQuote, List<Quote> newQuoteListQueue)
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

                        Quote relatedQuoteUpdate = _quoteCollection.Find(x => x._id == relatedQuoteId).FirstOrDefault();

                        foreach (Quote newQuote in newQuoteListQueue)
                        {
                            if (_relationship.ParentId != newQuote._id)
                            {
                                Quote quoteToUpdate = _quoteCollection.Find(x => x._id == newQuote._id).FirstOrDefault();

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
                                var result = _quoteCollection.ReplaceOne(filter, quoteToUpdate, new UpdateOptions() { IsUpsert = true }, _cancellationToken);
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
