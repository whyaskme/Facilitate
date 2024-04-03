using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using static Facilitate.Libraries.Models.Constants.Messaging;

namespace AdminBlazor.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebServices : ControllerBase
    {
        Utils utils = new Utils();

        string dbName = "Facilitate";
        string collectionName = "Quote";

        string resultMsg = string.Empty;

        //string mongoUri = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate";
        string mongoUri = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate";

        IMongoClient client;

        IMongoCollection<Quote> collection;
        private CancellationToken _cancellationToken;

        // GET: api/<WebServices>
        //[HttpGet]
        [Route("GetQuotes")]
        public List<Quote> GetQuotes(string status)
        {
            List<Attachment> unSortedFiles = new List<Attachment>();
            List<Note> unSortedNotes = new List<Note>();
            List<Event> unSortedEvents = new List<Event>();

            try
            {
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f.status, status);

                var sortedQuotes = collection.Find(filter).SortByDescending(e => e.timestamp).ToList();
                for (var i = 0; i < sortedQuotes.Count; i++)
                {
                    unSortedFiles.Clear();
                    unSortedNotes.Clear();
                    unSortedEvents.Clear();

                    // Convert to local time
                    sortedQuotes[i].timestamp = sortedQuotes[i].timestamp.ToLocalTime();
                    sortedQuotes[i].lastUpdated = sortedQuotes[i].lastUpdated.ToLocalTime();

                    for (var j = 0; j < sortedQuotes[i].attachments.Count; j++)
                    {
                        Attachment currentAttachment = sortedQuotes[i].attachments[j];

                        var currentDateTime = currentAttachment.Date;

                        currentAttachment.Date = currentAttachment.Date.ToLocalTime();

                        unSortedFiles.Add(currentAttachment);
                    }

                    for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
                    {
                        Note currentNote = sortedQuotes[i].notes[j];
                        currentNote.Date = currentNote.Date.ToLocalTime();

                        unSortedNotes.Add(currentNote);
                    }

                    for (var j = 0; j < sortedQuotes[i].events.Count; j++)
                    {
                        Event currentEvent = sortedQuotes[i].events[j];
                        currentEvent.DateTime = currentEvent.DateTime.ToLocalTime();

                        unSortedEvents.Add(currentEvent);
                    }

                    sortedQuotes[i].attachments = SortFilesByDateDesc(unSortedFiles);
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

        // GET api/<WebServices>/5
        [HttpGet("{id}")]
        public string GetQuote(int id)
        {
            return "value";
        }

        // POST api/<WebServices>
        [HttpPost]
        public void PostQuote([FromBody] string value)
        {
        }

        // PUT api/<WebServices>/5
        [HttpPut("{id}")]
        public void PutQuote(int id, [FromBody] string value)
        {
        }

        [HttpPut("{quote}")]
        public string UpdateQuote(Quote quote)
        {
            string quoteId = quote._id;

            quote.lastUpdated = DateTime.UtcNow;

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

        [HttpDelete("{quoteId}, {quote}")]
        public string DeleteQuote(string quoteId)
        {
            try
            {
                // This is a soft delete > move to archive.
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                // Get the selected quote for update
                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f._id, quoteId);

                var sortedelectedQuote = collection.Find(filter).ToList()[0];

                Event _event = new Event(0, 0);
                _event.Details = sortedelectedQuote.status + " moved to Archive";

                sortedelectedQuote.status = "Archive";
                sortedelectedQuote.events.Add(_event);

                var result = collection.ReplaceOne(filter, sortedelectedQuote, new UpdateOptions() { IsUpsert = true }, _cancellationToken);

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

        public QuoteLeaderboard GetLeaderBoardStats()
        {
            QuoteLeaderboard quoteLeaderboard = new QuoteLeaderboard();

            try
            {
                client = new MongoClient(mongoUri);
                collection = client.GetDatabase(dbName).GetCollection<Quote>(collectionName);

                var countsByQuoteStatus = collection.Aggregate()
                          .Group(
                              x => x.status,
                              g => new QuoteStat
                              {
                                  QuoteType = g.Key,
                                  QuoteCount = g.Count(),
                                  QuoteValue = g.Sum(x => x.totalQuote)
                              }).ToList();

                foreach (var item in countsByQuoteStatus)
                {
                    if (item.QuoteType == "New")
                    {
                        quoteLeaderboard.LeadCount = item.QuoteCount;
                        quoteLeaderboard.LeadValue = item.QuoteValue;
                    }
                    else if (item.QuoteType == "Opportunity")
                    {
                        quoteLeaderboard.OpportunityCount = item.QuoteCount;
                        quoteLeaderboard.OpportunityValue = item.QuoteValue;
                    }
                    else if (item.QuoteType == "Customer")
                    {
                        quoteLeaderboard.CustomerCount = item.QuoteCount;
                        quoteLeaderboard.CustomerValue = item.QuoteValue;
                    }
                    else if (item.QuoteType == "Complete")
                    {
                        quoteLeaderboard.CompletionCount = item.QuoteCount;
                        quoteLeaderboard.CompletionValue = item.QuoteValue;
                    }
                    else if (item.QuoteType == "Archive")
                    {
                        quoteLeaderboard.ArchiveCount = item.QuoteCount;
                        quoteLeaderboard.ArchiveValue = item.QuoteValue;
                    }
                    else if (item.QuoteType == "Warranty")
                    {
                        quoteLeaderboard.WarrantyCount = item.QuoteCount;
                        quoteLeaderboard.WarrantyValue = item.QuoteValue;
                    }
                }   
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }

            quoteLeaderboard.TotalQuoteCount = quoteLeaderboard.LeadCount + quoteLeaderboard.OpportunityCount + quoteLeaderboard.CustomerCount + quoteLeaderboard.CompletionCount + quoteLeaderboard.ArchiveCount + quoteLeaderboard.WarrantyCount;
            quoteLeaderboard.TotalQuoteValue = quoteLeaderboard.LeadValue + quoteLeaderboard.OpportunityValue + quoteLeaderboard.CustomerValue + quoteLeaderboard.CompletionValue + quoteLeaderboard.ArchiveValue + quoteLeaderboard.WarrantyValue;

            return quoteLeaderboard;
        }

        public List<Event> SortEventsByDateDesc(List<Event> originalList)
        {
            return originalList.OrderByDescending(x => x.DateTime).ToList();
        }

        public List<Attachment> SortFilesByDateDesc(List<Attachment> originalList)
        {
            return originalList.OrderByDescending(x => x.Date).ToList();
        }

        public List<Note> SortNotesByDateDesc(List<Note> originalList)
        {
            return originalList.OrderByDescending(x => x.Date).ToList();
        }
    }
}
