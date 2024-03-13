using Facilitate.Libraries.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AdminBlazor.Data {

    public class QuoteService
    {
        string dbName = "Facilitate";
        string collectionName = "Quote";

        string resultMsg = string.Empty;

        //string mongoUri = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate";
        string mongoUri = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate";

        IMongoClient client;

        IMongoCollection<Quote> collection;

        public IEnumerable<Quote> GetQuotes()
        {
            try
            {
                client = new MongoClient(mongoUri);

                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var allQuotes = collection.Find(Builders<Quote>.Filter.Empty).ToList();

                return allQuotes;
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }
            return null;
        }
    }
}