using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using Libraries.Models;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotesController : ControllerBase
    {
        string dbName = "Facilitate";
        string collectionName = "Quote";

        string resultMsg = string.Empty;
        //string mongoUri = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate";
        string mongoUri = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate";

        IMongoClient client;

        IMongoCollection<Quote> collection;

        private readonly ILogger<QuotesController> _logger;

        public QuotesController(ILogger<QuotesController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetQuotes")]
        public IEnumerable<Quote> GetQuotes()
        {
            try
            {
                client = new MongoClient(mongoUri);

                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var allQuotes = collection.Find(Builders<Quote>.Filter.Empty).ToList();

                return allQuotes;
            }
            catch(Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }

            return null;
        }

        [HttpPost(Name = "PostQuote")]
        public string PostQuote(Quote quote)
        {
            quote._t = "Quote";
            quote._id = ObjectId.GenerateNewId().ToString();

            var bDebug = true;
            if (bDebug)
            {
                quote.address = "2155 Old Highway 8, New Brighton, Minnesota 55112";
                quote.fullAddress = "2155 Old Highway 8, New Brighton, Minnesota 55112";
                quote.street = "2155 Old Highway 8";
                quote.city = "New Brighton";
                quote.state = "Minnesota";
                quote.zip = "55112";
                quote.firstName = "Home";
                quote.lastName = "Owner";
                quote.email = "help@roofle.com";
                quote.phone = "(612) 255-8200";
                quote.market = "Minneapolis-St. Paul, MN";
                quote.externalUrl = "https://app.roofle.com/dashboard";
                quote.timestamp = "2023-07-19T16:48:22.193Z";
                quote.numberOfStructures = 1;
                quote.numberOfIncludedStructures = 1;
                quote.totalSquareFeet = 1369;
                quote.mainRoofTotalSquareFeet = 1369;
                quote.totalInitialSquareFeet = 1369;
                quote.sessionId = "nH9YvHwoBldl2ZkpQSWrX";

                // Add Product Info
                Product product = new Product();
                product.name = "Certainteed Landmark";
                product.id = 1;

                PriceInfo _priceInfo = new PriceInfo();
                _priceInfo.priceType = "BasicFinancing";
                _priceInfo.total = 15069.5;
                _priceInfo.pricePerSquare = 401;
                _priceInfo.monthly = 334;
                _priceInfo.apr = 26.1;
                _priceInfo.months = 180;
                product.priceInfo = _priceInfo;

                PriceRange _priceRange = new PriceRange();
                _priceRange.totalMin = 13562;
                _priceRange.totalMax = 16575;
                _priceRange.monthlyMin = 300;
                _priceRange.monthlyMax = 367;
                product.priceRange = _priceRange;

                product.wasteFactorMainRoof = 1.2;

                quote.products = null;
                quote.products = new List<Product>();
                quote.products.Add(product);

                quote.repLead = "Matt Roofer";
                quote.repEmail = "admin@facilitate.org";
                quote.leadId = 123;

                // Add Structure Info
                Structure structure = new Structure();
                structure.name = "Main Roof";
                structure.slope = "medium";
                structure.isIncluded = true;
                structure.squareFeet = 1369;
                structure.initialSquareFeet = 1369;
                structure.roofComplexity = "Simple";

                quote.structures = null;
                quote.structures = new List<Structure>();
                quote.structures.Add(structure);
            }

            var results = string.Empty;

            try
            {
                client = new MongoClient(mongoUri);

                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);
                collection.InsertOne(quote);
            }
            catch (Exception ex)
            {
                results = ex.Message;
            }
            finally
            {
                results = "POSTED";
            }

            return results;
        }

        [HttpPut(Name = "PutQuote")]
        public string PutQuote(Quote quote)
        {
            var results = string.Empty;

            try
            {

            }
            catch (Exception ex)
            {
                results = ex.Message;
            }
            finally
            {
                results = "UPDATED";
            }

            return results;
        }

        //[HttpDelete(Name = "DeleteQuote")]
        //public string DeleteQuote(string quoteId)
        //{
        //    var results = string.Empty;

        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        results = ex.Message;
        //    }
        //    finally
        //    {
        //        results = "DELETED";
        //    }

        //    return results;
        //}

        [HttpDelete(Name = "DeleteQuote")]
        public string DeleteQuote()
        {
            var results = string.Empty;

            try
            {
                client = new MongoClient(mongoUri);

                var db = client.GetDatabase(dbName);
                db.DropCollection(collectionName);
            }
            catch (Exception ex)
            {
                results = ex.Message;
            }
            finally
            {
                results = "DELETED";
            }

            return results;
        }
    }
}
