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

        // GET api/<TestController>/5
        [HttpGet("{numQuotesToCreate}")]
        public IActionResult Get(string Trade, int numQuotesToCreate)
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

            string headerForwardedFor = "n/a";
            string headerReferer = "n/a";

            int childBidderQuotesToCreate = 1;
            int BiddingExpiresInDays = 1;

            double AvgPricePerSqFt = 4.50;
            double totalQuoteValue = 0;

            var requestHeaders = HttpContext.Request.Headers;

            List<String> nameGenders = new List<string>();
            nameGenders.Add("male");
            nameGenders.Add("female");

            Random rnd = new Random();
            int randomInt = rnd.Next(0, 1);

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

                for (var i = 0; i < numQuotesToCreate; i++)
                {
                    try
                    {
                        if (i == 0)
                        {
                            // Create the parent Aggregate quote
                            aggregateQuote = new Quote();
                            aggregateQuote.Trade = utils.TitleCaseString("Aggregate");

                            // Set Bidding properties
                            aggregateQuote.Bidder = author;
                            aggregateQuote.BidderType = "Parent";
                            aggregateQuote.BiddingExpires = DateTime.UtcNow.AddDays(BiddingExpiresInDays);

                            aggregateQuote.ipAddress = headerForwardedFor;
                            aggregateQuote.externalUrl = headerReferer;

                            aggregateQuote.firstName = firstName;
                            aggregateQuote.lastName = lastName;

                            aggregateQuote.email = aggregateQuote.firstName.ToLower() + "@" + aggregateQuote.lastName.ToLower() + ".com";
                            aggregateQuote.phone = "(" + utils.GetRandomAreaCode() + ") " + utils.GetRandomHomePhoneNumber();
                            aggregateQuote.market = aggregateQuote.city + ", " + aggregateQuote.state;

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
                                    case 1:
                                        structure.initialSquareFeet = rnd.Next(1000, 5000);
                                        structure.isIncluded = true;
                                        structure.name = "Main Roof";
                                        structure.roofComplexity = "Complex";
                                        structure.slope = "steep";
                                        break;
                                    case 2:
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

                            aggregateQuote.totalQuote = (totalSquareFeet*AvgPricePerSqFt);

                            aggregateQuote.address = randomStreetNumber + " " + randomStreetName + ", " + cityName + ", " + stateAbbr + " " + randomZipCode;
                            aggregateQuote.fullAddress = aggregateQuote.address;
                            aggregateQuote.street = randomStreetNumber + " " + randomStreetName;
                            aggregateQuote.city = cityName;

                            aggregateQuote.state = stateAbbr;
                            aggregateQuote.zip = randomZipCode;

                            aggregateQuote.timestamp = DateTime.UtcNow;

                            randomInt = rnd.Next(0, 1);
                            repName = utils.GetRandomFirstName(nameGenders[randomInt]) + " " + utils.GetRandomLastName();

                            aggregateQuote.repName = repName;
                            aggregateQuote.repEmail = repName.Replace(" ", ".").ToLower() + "@facilitate.org";

                            randomInt = rnd.Next(5000, 9999);
                            aggregateQuote.leadId = randomInt;

                            CreateParentSpawnedEvent(aggregateQuote);

                            _quoteCollection.InsertOne(aggregateQuote);
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

                resultMsg = numQuotesToCreate + " Quotes added";
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
                        siblingRelationship.Name = newQuote.Trade;

                        quoteToUpdate.relationships.Add(siblingRelationship);

                        Event siblingEvent = new Event();
                        siblingEvent.Author = author;
                        siblingEvent.Trade = quoteToUpdate.Trade;
                        siblingEvent.Name = "Sibling (" + quoteToUpdate.Trade + ") quote Linked";
                        siblingEvent.Details = siblingEvent.Name + " to Sibling Id (" + siblingEvent._id + ")";

                        // Add event to child quote
                        quoteToUpdate.events.Add(siblingEvent);
                    }

                    var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteId);

                    var result = _quoteCollection.ReplaceOne(filter, quoteToUpdate, new UpdateOptions() { IsUpsert = true }, _cancellationToken);
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

    }
}
