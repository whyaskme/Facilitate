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
        private readonly ILogger<QuotesController> _logger;

        public QuotesController(ILogger<QuotesController> logger)
        {
            _logger = logger;
        }


        [HttpGet(Name = "GetQuotes")]
        public IEnumerable<Quote> GetQuotes()
        {
            var resultMsg = string.Empty;
            var mongoUri = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate";

            IMongoClient client;

            IMongoCollection<Quote> collection;

            try
            {
                client = new MongoClient(mongoUri);

                // Provide the name of the database and collection you want to use.
                // If they don't already exist, the driver and Atlas will create them
                // automatically when you first write data.
                var dbName = "Facilitate";
                var collectionName = "Quote";

                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                foreach(Quote doc in collection.Find(new BsonDocument()).ToList())
                {
                    Console.WriteLine(doc);
                }

                /*      *** INSERT DOCUMENTS ***
                 * 
                 * You can insert individual documents using collection.Insert(). 
                 * In this example, we're going to create 4 documents and then 
                 * insert them all in one call with InsertMany().
                 */

                //var docs = Recipe.GetRecipes();

                resultMsg = "Success";
            }
            catch(Exception ex)
            {
                //Console.WriteLine("There was a problem connecting to your " +
                //    "Atlas cluster. Check that the URI includes a valid " +
                //    "username and password, and that your IP address is " +
                //    $"in the Access List. Message: {ex.Message}");
                //Console.WriteLine(ex);
                //Console.WriteLine();
                resultMsg = ex.Message;
            }
            finally
            {

            }

            //// Provide the name of the database and collection you want to use.
            //// If they don't already exist, the driver and Atlas will create them
            //// automatically when you first write data.
            //var dbName = "Facilitate";
            //var collectionName = "ReferenceData";

            //collection = client.GetDatabase(dbName)
            //   .GetCollection<Recipe>(collectionName);

            ///*      *** INSERT DOCUMENTS ***
            // * 
            // * You can insert individual documents using collection.Insert(). 
            // * In this example, we're going to create 4 documents and then 
            // * insert them all in one call with InsertMany().
            // */

            //var docs = Recipe.GetRecipes();

            //try
            //{
            //    collection.InsertMany(docs);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine($"Something went wrong trying to insert the new documents." +
            //        $" Message: {e.Message}");
            //    Console.WriteLine(e);
            //    Console.WriteLine();
            //    return;
            //}

            return null;
        }

        [HttpPost(Name = "PostQuote")]
        public string PostQuote(Quote quote)
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
                results = "New quote (" + quote._id + ") for (" + quote.Consumer.FirstName + " " + quote.Consumer.LastName + ") POSTED";
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
                results = "Old quote (" + quote._id + ") for (" + quote.Consumer.FirstName + " " + quote.Consumer.LastName + ") UPDATED";
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
