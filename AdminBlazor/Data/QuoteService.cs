using AdminBlazor.Components.Pages;
using DevExpress.Export.Xl;
using Facilitate.Libraries.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Globalization;
using System.Reactive;
using System.Threading;

using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;

namespace AdminBlazor.Data {

    public class QuoteService
    {
        string dbName = "Facilitate";
        string collectionName = "Quote";

        string resultMsg = string.Empty;

        //string mongoUri = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate";
        //string mongoUri = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate";
        string mongoUri = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate";

        IMongoClient client;

        IMongoCollection<Quote> collection;
        private CancellationToken _cancellationToken;

        public List<Event> SortEventsByDateDesc(List<Event> originalList)
        {
            return originalList.OrderByDescending(x => x.DateTime).ToList();
        }

        public List<Note> SortNotesByDateDesc(List<Note> originalList)
        {
            return originalList.OrderByDescending(x => x.Date).ToList();
        }

        public IEnumerable<Quote> GetQuotes(string status)
        {
            List<Event> unSortedEvents = new List<Event>();
            List<Note> unSortedNotes = new List<Note>();

            try
            {
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f.status, status);

                var sortedQuotes = collection.Find(filter).SortByDescending(e => e.timestamp).ToList();
                for (var i = 0; i < sortedQuotes.Count; i++)
                {
                    unSortedEvents.Clear();
                    unSortedNotes.Clear();

                    for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
                    {
                        unSortedNotes.Add(sortedQuotes[i].notes[j]);
                    }

                    for (var j = 0; j < sortedQuotes[i].events.Count; j++)
                    {
                        unSortedEvents.Add(sortedQuotes[i].events[j]);
                    }

                    sortedQuotes[i].notes = SortNotesByDateDesc(unSortedNotes);
                    sortedQuotes[i].events = SortEventsByDateDesc(unSortedEvents);
                }

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

                var projectManager = quote.projectManager;

                Event _event = new Event(0, 0);
                _event.Details = "Lead assigned to Project Manager Id: " + projectManager._id + " (" + projectManager.Name + "), moved to Opportunities and emailed to: " + projectManager.Email;

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

        public string UpdateQuote(string quoteId, Quote quote)
        {
            try
            {
                // This is a soft delete > move to archive.
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteId);

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

        public string MakeCustomer(string quoteId, Quote quote)
        {
            try
            {
                // This is a soft delete > move to archive.
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var updateQuote = Builders<Quote>.Update.Set(quote => quote.status, "Opportunity");

                var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteId);

                var projectManager = quote.projectManager;

                Event _event = new Event(0, 0);
                _event.Details = "Opportunity converted to Customer";

                quote.status = "Customer";
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
                _event.Details = quote.status + " moved to Archive";

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