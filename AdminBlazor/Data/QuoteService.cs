using AdminBlazor.Components.Pages;
using DevExpress.Export.Xl;
using Facilitate.Libraries.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using System.Reactive;
using System.Threading;

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
        private CancellationToken _cancellationToken;

        public IEnumerable<Quote> GetQuotes(string status)
        {
            try
            {
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f.status, status);

                //var allQuotes = collection.Find(filter).ToList();

                var sortedQuotes = collection.Find(filter).SortByDescending(e => e.timestamp).ToList();

                return sortedQuotes;
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

        public Quote GetQuoteDetails(string objectId)
        {
            try
            {
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f._id, objectId);

                var _quote = collection.Find(filter).ToList();

                return (Quote)_quote[0];
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

        public int GetQuoteCount(string status)
        {
            try
            {
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f.status, status);

                var count = collection.CountDocuments(filter);

                return (int)count;
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }
            return 0;
        }

        public string CreateQuote(Quote quote)
        {
            try
            {
                client = new MongoClient(mongoUri);

                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);
                collection.InsertOne(quote);

                resultMsg = "Added Quote!";
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }
            return resultMsg;
        }

        public string AssignQuote(string quoteId, Quote quote)
        {
            try
            {
                // This is a soft delete > move to archive.
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var updateQuote = Builders<Quote>.Update.Set(quote => quote.status, "Opportunity");

                var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteId);

                Event _event = new Event(0,0);
                _event.Details = "Quote moved to Opportunities";

                quote.status = "Opportunity";
                quote.events.Add(_event);

                var result = collection.ReplaceOne(filter, quote, new UpdateOptions() { IsUpsert = true }, _cancellationToken);
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }
            return resultMsg;
        }

        public string DeleteQuote(string quoteId, Quote quote)
        {
            try
            {
                // This is a soft delete > move to archive.
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var updateQuote = Builders<Quote>.Update.Set(quote => quote.status, "Archive");

                var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteId);

                Event _event = new Event(0, 0);
                _event.Details = "Quote moved to Archive";

                quote.status = "Archive";
                quote.events.Add(_event);

                var result = collection.ReplaceOne(filter, quote, new UpdateOptions() { IsUpsert = true }, _cancellationToken);

                resultMsg = "Archived!";
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }
            return resultMsg;
        }

        public string DeleteAllQuotes()
        {
            try
            {
                client = new MongoClient(mongoUri);

                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);
                
                //client.GetDatabase(dbName).DropCollection("Quotes");

                //var result = collection.DeleteManyAsync({});

                resultMsg = "Deleted!";
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }
            return resultMsg;
        }
    }
}