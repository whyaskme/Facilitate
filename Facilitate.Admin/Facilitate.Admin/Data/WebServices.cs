using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;

using MongoDB.Driver;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using static Facilitate.Libraries.Models.Constants.Messaging;

namespace Facilitate.Admin.Data
{
    [ApiController]
    public class WebServices : ControllerBase
    {

        Utils utils = new Utils();

        public HttpClient _apiClient = new HttpClient();

        string _mongoDBName = "Facilitate";
        string _mongoDBCollectionName = "Quote";

        string resultMsg = string.Empty;

        List<Attachment> unSortedFiles = new List<Attachment>();
        List<Note> unSortedNotes = new List<Note>();
        List<Event> unSortedEvents = new List<Event>();

        //string _mongoDBConnectionString = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate";
        string _mongoDBConnectionString = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate;safe=true;maxpoolsize=200";

        IMongoClient _mongoDBClient;

        IMongoCollection<Quote> _mongoDBCollection;
        private CancellationToken _cancellationToken;

        public WebServices()
        {
            _mongoDBClient = new MongoClient(_mongoDBConnectionString);
            _mongoDBCollection = _mongoDBClient.GetDatabase(_mongoDBName).GetCollection<Quote>(_mongoDBCollectionName);

            _apiClient.BaseAddress = new Uri("https://api.facilitate.org/api");
        }

        public List<Quote> quoteList = new List<Quote>();

        public async Task<List<Quote>> CallQuoteApi(string status)
        {
            status = utils.TitleCaseString(status);

            var apiUrl = new Uri(_apiClient.BaseAddress + "/api/quote?status=" + status);

            //var quotes = await _apiClient.GetAsync(apiUrl);
            var quotes = await _apiClient.GetFromJsonAsync<List<Quote>>(apiUrl);

            return quotes;
        }

        public List<Quote> GetQuoteList(string status)
        {
            status = utils.TitleCaseString(status);

            var apiUrl = new Uri(_apiClient.BaseAddress + "/api/quote?status=" + status);

            var quotes = _apiClient.GetAsync(apiUrl);
            //var quotes = _apiClient.GetFromJsonAsync<Quote>(apiUrl);

            return null;
        }

        public List<Quote> GetQuotes(string status)
        {
            List<Quote> sortedQuotes = new List<Quote>();
            try
            {
                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f.status, status);

                //sortedQuotes = _mongoDBCollection.Find(filter).SortByDescending(e => e.timestamp).ToList();

                sortedQuotes = _mongoDBCollection.FindAsync(filter).Result.ToList();

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
            
            return sortedQuotes;
        }

        //public Quote GetQuote(Quote selectedQuote)
        //{
        //    try
        //    {
        //        var builder = Builders<Quote>.Filter;
        //        var filter = builder.Eq(f => f._id, selectedQuote._id);

        //        selectedQuote = (Quote)_mongoDBCollection.Find(filter);

        //        selectedQuote = SortItemsByDateDesc(selectedQuote);


        //        return selectedQuote;
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMsg = ex.Message;
        //    }
        //    finally
        //    {

        //    }
        //    return null;
        //}

        //public string CreateQuote(Quote quote)
        //{
        //    try
        //    {
        //        _mongoDBCollection.InsertOne(quote);

        //        resultMsg = "Added Quote!";
        //    }
        //    catch (Exception ex)
        //    {
        //        resultMsg = ex.Message;
        //    }
        //    finally
        //    {

        //    }
        //    return resultMsg;
        //}

        public async Task<string> CreateQuoteApi(int numQuotesToCreate)
        {
            try
            {
                var apiUrl = new Uri(_apiClient.BaseAddress + "/api/Test/" + numQuotesToCreate);

                resultMsg = await _apiClient.GetStringAsync(apiUrl);

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

        [HttpPut("{quote}")]
        public Quote UpdateQuote(Quote quote)
        {
            string quoteId = quote._id;

            quote.lastUpdated = DateTime.UtcNow;

            try
            {
                var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteId);

                var result = _mongoDBCollection.ReplaceOne(filter, quote, new UpdateOptions() { IsUpsert = true }, _cancellationToken);
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            finally
            {

            }

            //quote = SortItemsByDateDesc(quote);

            return quote;
        }

        public int GetQuoteCount(string status)
        {
            try
            {
                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f.status, status);

                var count = _mongoDBCollection.CountDocuments(filter);

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
                var countsByQuoteStatus = _mongoDBCollection.Aggregate()
                          .Group(
                              x => x.status,
                              g => new QuoteStat
                              {
                                  QuoteType = g.Key,
                                  QuoteCount = g.Count(),
                                  QuoteValue = g.Sum(x => x.totalQuote),
                                  QuoteSqFt = g.Sum(x => x.totalSquareFeet)
                              }).ToList();

                foreach (var item in countsByQuoteStatus)
                {
                    if (item.QuoteType == "New")
                    {
                        quoteLeaderboard.LeadCount = item.QuoteCount;
                        quoteLeaderboard.LeadValue = item.QuoteValue;
                        quoteLeaderboard.LeadSqFt = item.QuoteSqFt;
                    }
                    else if (item.QuoteType == "Opportunity")
                    {
                        quoteLeaderboard.OpportunityCount = item.QuoteCount;
                        quoteLeaderboard.OpportunityValue = item.QuoteValue;
                        quoteLeaderboard.OpportunitySqFt = item.QuoteSqFt;
                    }
                    else if (item.QuoteType == "Customer")
                    {
                        quoteLeaderboard.CustomerCount = item.QuoteCount;
                        quoteLeaderboard.CustomerValue = item.QuoteValue;
                        quoteLeaderboard.CustomerSqFt = item.QuoteSqFt;
                    }
                    else if (item.QuoteType == "Complete")
                    {
                        quoteLeaderboard.CompletionCount = item.QuoteCount;
                        quoteLeaderboard.CompletionValue = item.QuoteValue;
                        quoteLeaderboard.CompletionSqFt = item.QuoteSqFt;
                    }
                    else if (item.QuoteType == "Archive")
                    {
                        quoteLeaderboard.ArchiveCount = item.QuoteCount;
                        quoteLeaderboard.ArchiveValue = item.QuoteValue;
                        quoteLeaderboard.ArchiveSqFt = item.QuoteSqFt;
                    }
                    else if (item.QuoteType == "Warranty")
                    {
                        quoteLeaderboard.WarrantyCount = item.QuoteCount;
                        quoteLeaderboard.WarrantyValue = item.QuoteValue;
                        quoteLeaderboard.WarrantySqFt = item.QuoteSqFt;
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
            quoteLeaderboard.TotalQuoteSqFt = quoteLeaderboard.LeadSqFt + quoteLeaderboard.OpportunitySqFt + quoteLeaderboard.CustomerSqFt + quoteLeaderboard.CompletionSqFt + quoteLeaderboard.ArchiveSqFt + quoteLeaderboard.WarrantySqFt;

            return quoteLeaderboard;
        }

        public Quote SortItemsByDateDesc(Quote quote)
        {
            quote.attachments = SortFilesByDateDesc(quote.attachments);
            quote.notes = SortNotesByDateDesc(quote.notes);
            quote.events = SortEventsByDateDesc(quote.events);

            return quote;
        }

        public List<Event> SortEventsByDateDesc(List<Event> originalList)
        {
            if(originalList.Count > 0)
                return originalList.OrderByDescending(x => x.DateTime).ToList();
            else return originalList;
        }

        public List<Attachment> SortFilesByDateDesc(List<Attachment> originalList)
        {
            if (originalList.Count > 0)
                return originalList.OrderByDescending(x => x.Date).ToList();
            else return originalList;
        }

        public List<Note> SortNotesByDateDesc(List<Note> originalList)
        {
            if (originalList.Count > 0)
                return originalList.OrderByDescending(x => x.Date).ToList();
            else return originalList;
        }
    }
}
