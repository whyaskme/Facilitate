using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

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
        //string mongoUri = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate";
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

                //foreach (Quote quote in allQuotes)
                //{
                //    var id = quote._id;
                //}

                resultMsg = "Success";
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
                results = "New quote for (" + quote.FirstName + " " + quote.LastName + ") POSTED";
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
                results = "Old quote for (" + quote.FirstName + " " + quote.LastName + ") UPDATED";
            }

            return results;
        }

        [HttpDelete(Name = "DeleteQuote")]
        public string DeleteQuote(string quoteId)
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
                results = "Old quote (" + quoteId + ") DELETED";
            }

            return results;
        }
    }
}
