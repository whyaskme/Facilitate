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
    public class TestController : ControllerBase
    {

        private readonly Utils utils;

        string _mongoDBCollectionName = "Quote";
        string resultMsg = string.Empty;

        private readonly IMongoDatabase _mongoDatabase;
        private readonly IMongoCollection<Quote> _quoteCollection;

        public TestController(DBService dBService, Utils utils)
        {
            _mongoDatabase = dBService.MongoDatabase;
            _quoteCollection = _mongoDatabase.GetCollection<Quote>(_mongoDBCollectionName);
            this.utils = utils;
        }

        // GET api/<TestController>/5
        [HttpGet("{numQuotesToCreate}")]
        public async Task<IActionResult> Get(string applicationType, int numQuotesToCreate, CancellationToken ct)
        {
            List<String> nameGenders = new List<string>();
            nameGenders.Add("male");
            nameGenders.Add("female");

            try
            {
                for (var i = 0; i < numQuotesToCreate; i++)
                {
                    try
                    {
                        Quote quote = new Quote();
                        quote.status = "New";

                        // Will need to figure out how to set dynamically
                        quote.applicationType = utils.TitleCaseString(applicationType);

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

                        var randomZipCode = utils.GetRandomZipCodeAsync(MongoDB.Bson.ObjectId.Parse(cityId)).ToString();

                        quote.address = randomStreetNumber + " " + randomStreetName + ", " + cityName + ", " + stateAbbr + " " + randomZipCode;
                        quote.fullAddress = quote.address;
                        quote.street = randomStreetNumber + " " + randomStreetName;
                        quote.city = cityName;
                        quote.state = stateAbbr;
                        quote.zip = randomZipCode;

                        Random rnd = new Random();
                        int randomInt = rnd.Next(0, 1);

                        quote.firstName = await utils.GetRandomFirstNameAsync(nameGenders[randomInt], ct);
                        quote.lastName = await utils.GetRandomLastNameAsync(ct);

                        quote.email = quote.firstName.ToLower() + "@" + quote.lastName.ToLower() + ".com";
                        quote.phone = "(" + utils.GetRandomAreaCode() + ") " + utils.GetRandomHomePhoneNumber();
                        quote.market = quote.city + ", " + quote.state;
                        quote.externalUrl = "Auto-generated WebApi";
                        quote.sessionId = "nH9YvHwoBldl2ZkpQSWrX";

                        randomInt = rnd.Next(0, 1);
                        var repName = (await utils.GetRandomFirstNameAsync(nameGenders[randomInt]), ct) + " " + (await utils.GetRandomLastNameAsync(ct));

                        quote.repName = repName;
                        quote.repEmail = repName.Replace(" ", ".").ToLower() + "@facilitate.org";

                        randomInt = rnd.Next(5000, 9999);
                        quote.leadId = randomInt;

                        // Add Structure Info
                        int totalSquareFeet = 0;
                        int mainRoofTotalSquareFeet = randomInt;

                        randomInt = rnd.Next(1, 3);

                        quote.numberOfStructures = randomInt;
                        quote.numberOfIncludedStructures = quote.numberOfStructures;

                        quote.structures = null;
                        quote.structures = new List<Structure>();
                        for (var j = 0; j < quote.numberOfStructures; j++)
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
                                    structure.initialSquareFeet = rnd.Next(500, 1000);
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

                            totalSquareFeet += structure.initialSquareFeet;

                            quote.structures.Add(structure);
                        }

                        quote.totalSquareFeet = totalSquareFeet;
                        quote.totalInitialSquareFeet = quote.totalSquareFeet;

                        quote.mainRoofTotalSquareFeet = totalSquareFeet;

                        quote.products = null;
                        quote.products = new List<Product>();

                        // Add Product Info
                        var productIndex = 0;
                        double quoteTotal = 0;

                        foreach (Structure structure in quote.structures)
                        {
                            productIndex++;

                            Product product = new Product();
                            product.name = "Certainteed Landmark (" + productIndex + ")";
                            product.id = 1;

                            PriceInfo _priceInfo = new PriceInfo();
                            _priceInfo.priceType = "BasicFinancing";

                            randomInt = rnd.Next(550, 675);
                            _priceInfo.pricePerSquare = randomInt;

                            randomInt = rnd.Next(250, 400);
                            _priceInfo.monthly = randomInt;

                            randomInt = rnd.Next(8, 26);
                            _priceInfo.apr = randomInt;

                            randomInt = rnd.Next(120, 360);
                            _priceInfo.months = randomInt;

                            product.priceInfo = _priceInfo;

                            PriceRange _priceRange = new PriceRange();
                            randomInt = rnd.Next(1150, 5200);
                            _priceRange.totalMin = randomInt;

                            randomInt = rnd.Next(16575, 16575);
                            _priceRange.totalMax = randomInt;

                            randomInt = rnd.Next(200, 300);
                            _priceRange.monthlyMin = randomInt;

                            randomInt = rnd.Next(350, 400);
                            _priceRange.monthlyMax = randomInt;

                            product.priceRange = _priceRange;

                            product.wasteFactorMainRoof = 1.2;

                            _priceInfo.total = (_priceInfo.pricePerSquare * quote.totalSquareFeet) / 100;

                            if (quoteTotal < _priceInfo.total)
                                quoteTotal = _priceInfo.total;

                            quote.products.Add(product);
                        }

                        quote.totalQuote = quoteTotal;

                        Event _event = new Event();
                        _event.Name = "New Quote";
                        _event.DateTime = DateTime.UtcNow;
                        _event.Details = "Admin generated test Quote";

                        //_event.Author = currentUser;

                        if (_event != null)
                            quote.events.Add(_event);

                        await _quoteCollection.InsertOneAsync(quote, null, ct);
                    }
                    catch (Exception ex)
                    {
                        resultMsg = ex.Message;
                    }
                    finally
                    {
                    }
                }

                //_quoteCollection.InsertOne(quote);

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

    }
}
