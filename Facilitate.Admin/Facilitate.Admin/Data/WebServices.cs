using DevExpress.Office.Utils;
using Facilitate.Admin.Components.Account;
using Facilitate.Admin.Components.Account.Pages;
using Facilitate.Libraries.Models;
using Json.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using ServiceStack;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Security.Cryptography;
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

        List<Libraries.Models.Attachment> unSortedFiles = new List<Libraries.Models.Attachment>();
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

        public List<QuoteHeader> GetSummaries(string status, bool showHideTestData)
        {
            try
            {
                var fields = Builders<Quote>.Projection.Include(p => p.firstName)
                    .Include(p => p.lastName)
                    .Include(p => p._id)
                    .Include(p => p.email)
                    .Include(p => p.status)
                    .Include(p => p.projectManager)
                    .Include(p => p.applicationType)
                    .Include(p => p.numberOfStructures)
                    .Include(p => p.numberOfIncludedStructures)
                    .Include(p => p.totalQuote)
                    .Include(p => p.totalSquareFeet)
                    .Include(p => p.timestamp)
                    .Include(p => p.lastUpdated)
                    .Include(p => p.street)
                    .Include(p => p.city)
                    .Include(p => p.state)
                    .Include(p => p.zip);

                var builder = Builders<Quote>.Filter;
                var filter = Builders<Quote>.Filter.Where(p => p.status.Contains(status));

                if (!showHideTestData)
                {
                    var dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.externalUrl, "Auto-generated WebApi"));

                    filter = Builders<Quote>.Filter.And(filter, dataSourceFilter);
                }

                List<QuoteHeader> quoteHeaders = _mongoDBCollection.Find(filter).Project<QuoteHeader>(fields).ToList();
                foreach (var quote in quoteHeaders)
                {
                    quote.lastUpdated = quote.lastUpdated.ToLocalTime();
                    quote.timestamp = quote.timestamp.ToLocalTime();
                }

                return quoteHeaders;
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }

            return null;
        }

        public List<QuoteHeader> GetSummaries(string tradeType, string status, bool showHideTestData)
        {
            try
            {
                var fields = Builders<Quote>.Projection.Include(p => p.firstName)
                    .Include(p => p.lastName)
                    .Include(p => p._id)
                    .Include(p => p.email)
                    .Include(p => p.status)
                    .Include(p => p.projectManager)
                    .Include(p => p.applicationType)
                    .Include(p => p.numberOfStructures)
                    .Include(p => p.numberOfIncludedStructures)
                    .Include(p => p.totalQuote)
                    .Include(p => p.totalSquareFeet)
                    .Include(p => p.timestamp)
                    .Include(p => p.lastUpdated)
                    .Include(p => p.street)
                    .Include(p => p.city)
                    .Include(p => p.state)
                    .Include(p => p.zip);

                var filterBuilder = Builders<Quote>.Filter;
                var filter = filterBuilder.Eq("status", status) & filterBuilder.Eq("applicationType", utils.TitleCaseString(tradeType));

                if (!showHideTestData)
                {
                    var dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.externalUrl, "Auto-generated WebApi"));

                    filter = Builders<Quote>.Filter.And(filter, dataSourceFilter);
                }

                List<QuoteHeader> quoteHeaders = _mongoDBCollection.Find(filter).Project<QuoteHeader>(fields).ToList();
                foreach(var quote in quoteHeaders)
                {
                    quote.lastUpdated = quote.lastUpdated.ToLocalTime();
                    quote.timestamp = quote.timestamp.ToLocalTime();
                }

                return quoteHeaders;
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }

            return null;
        }

        public List<QuoteHeader> GetSummaries(string status, string userId)
        {
            List<QuoteHeader> QuoteHeaders = new List<QuoteHeader>();
            try
            {
                var condition = Builders<Quote>.Filter.Eq(f => f.status, status);

                var fields = Builders<Quote>.Projection.Include(p => p.firstName)
                    .Include(p => p.lastName)
                    .Include(p => p.status)
                    .Include(p => p.numberOfStructures)
                    .Include(p => p.numberOfIncludedStructures)
                    .Include(p => p.totalQuote)
                    .Include(p => p.totalSquareFeet)
                    .Include(p => p.timestamp)
                    .Include(p => p.lastUpdated)
                    .Include(p => p.street)
                    .Include(p => p.city)
                    .Include(p => p.state)
                    .Include(p => p.zip);


                var builder = Builders<Quote>.Filter;
                var filter = Builders<Quote>.Filter.And(
                    Builders<Quote>.Filter.Where(p => p.status.Contains(status)),
                    Builders<Quote>.Filter.Where(p => p.projectManager.Email.Contains(userId))
                    );

                List<Quote> quotes = _mongoDBCollection.Find(filter).Project<Quote>(fields).ToList();

                for (var i = 0; i < quotes.Count; i++)
                {
                    QuoteHeader _header = new QuoteHeader();
                    _header._id = quotes[i]._id;
                    _header.firstName = quotes[i].firstName;
                    _header.lastName = quotes[i].lastName;
                    _header.status = quotes[i].status;

                    _header.street = quotes[i].street;
                    _header.city = quotes[i].city;
                    _header.state = quotes[i].state;
                    _header.zip = quotes[i].zip;

                    _header.numberOfStructures = quotes[i].numberOfStructures;
                    _header.numberOfIncludedStructures = quotes[i].numberOfIncludedStructures;

                    _header.totalQuote = quotes[i].totalQuote;
                    _header.totalSquareFeet = quotes[i].totalSquareFeet;

                    _header.timestamp = quotes[i].timestamp.ToLocalTime();
                    _header.lastUpdated = quotes[i].lastUpdated.ToLocalTime();

                    QuoteHeaders.Add(_header);
                }

                return QuoteHeaders;
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }

            return QuoteHeaders;
        }

        public List<QuoteSummary> GetChildQuoteSummaries(Quote quote)
        {
            List<QuoteSummary> QuoteHeaders = new List<QuoteSummary>();

            try
            {
                foreach(var relationship in quote.relationships)
                {
                    var childId = relationship._id;
                    var filter = Builders<Quote>.Filter.Eq(f => f._id, childId);

                    List<Quote> quotes = _mongoDBCollection.Find(filter).ToList();

                    for (var i = 0; i < quotes.Count; i++)
                    {
                        QuoteSummary _summary = new QuoteSummary();

                        _summary.relationship = quote.applicationType;
                        if (quote.applicationType == "Aggregate")
                            _summary.relationship = "Child";
                        else
                            _summary.relationship = "Parent";

                        _summary._id = quotes[i]._id;
                        _summary.applicationType = quotes[i].applicationType;
                        _summary.status = quotes[i].status;

                        _summary.events = quotes[i].events;

                        _summary.totalQuote = quotes[i].totalQuote;
                        _summary.projectManager = quotes[i].projectManager;

                        _summary.timestamp = quotes[i].timestamp.ToLocalTime();
                        _summary.lastEventTimeStamp = quotes[i].lastUpdated.ToLocalTime();

                        QuoteHeaders.Add(_summary);
                    }
                }

                return QuoteHeaders;
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }

            return QuoteHeaders;
        }

        public List<Quote> GetQuotes(string status, bool showHideTestData)
        {
            List<Quote> sortedQuotes = new List<Quote>();
            try
            {
                var builder = Builders<Quote>.Filter;
                var filter = Builders<Quote>.Filter.Where(p => p.status.Contains(status));
                var dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.externalUrl, "Auto-generated WebApi"));

                if (!showHideTestData)
                {
                    filter = Builders<Quote>.Filter.And(filter, dataSourceFilter);
                }

                sortedQuotes = _mongoDBCollection.Find(filter).SortByDescending(e => e.timestamp).ToList();

                for (var i = 0; i < sortedQuotes.Count; i++)
                {
                    if (sortedQuotes[i].firstName == "")
                    {
                        sortedQuotes[i].firstName = "Address";
                    }

                    if (sortedQuotes[i].lastName == "")
                    {
                        sortedQuotes[i].lastName = "Only";
                    }

                    unSortedFiles.Clear();
                    unSortedNotes.Clear();
                    unSortedEvents.Clear();

                    // Convert to local time
                    sortedQuotes[i].timestamp = sortedQuotes[i].timestamp.ToLocalTime();
                    sortedQuotes[i].lastUpdated = sortedQuotes[i].lastUpdated.ToLocalTime();

                    for (var j = 0; j < sortedQuotes[i].attachments.Count; j++)
                    {
                        Libraries.Models.Attachment currentAttachment = sortedQuotes[i].attachments[j];

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

        public List<Quote> GetQuotes(string tradeType, string status, bool showHideTestData)
        {
            List<Quote> sortedQuotes = new List<Quote>();
            try
            {
                var tradeFilter = Builders<Quote>.Filter.Where(p => p.applicationType.Contains(utils.TitleCaseString(tradeType)));

                var filterBuilder = Builders<Quote>.Filter;
                var filter = filterBuilder.Eq("status", status) & filterBuilder.Eq("applicationType", utils.TitleCaseString(tradeType));

                if (!showHideTestData)
                {
                    var dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.externalUrl, "Auto-generated WebApi"));

                    filter = Builders<Quote>.Filter.And(filter, dataSourceFilter);
                }

                sortedQuotes = _mongoDBCollection.Find(filter).SortByDescending(e => e.timestamp).ToList();

                for (var i = 0; i < sortedQuotes.Count; i++)
                {
                    if (sortedQuotes[i].firstName == "")
                    {
                        sortedQuotes[i].firstName = "Address";
                    }

                    if (sortedQuotes[i].lastName == "")
                    {
                        sortedQuotes[i].lastName = "Only";
                    }

                    unSortedFiles.Clear();
                    unSortedNotes.Clear();
                    unSortedEvents.Clear();

                    // Convert to local time
                    sortedQuotes[i].timestamp = sortedQuotes[i].timestamp.ToLocalTime();
                    sortedQuotes[i].lastUpdated = sortedQuotes[i].lastUpdated.ToLocalTime();

                    for (var j = 0; j < sortedQuotes[i].attachments.Count; j++)
                    {
                        Libraries.Models.Attachment currentAttachment = sortedQuotes[i].attachments[j];

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

        public List<Quote> GetQuotes(string status)
        {
            List<Quote> sortedQuotes = new List<Quote>();
            try
            {
                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f.status, status);

                sortedQuotes = _mongoDBCollection.Find(filter).SortByDescending(e => e.timestamp).ToList();
                for (var i = 0; i < sortedQuotes.Count; i++)
                {
                    if (sortedQuotes[i].firstName == "")
                    {
                        sortedQuotes[i].firstName = "Address";
                    }

                    if (sortedQuotes[i].lastName == "")
                    {
                        sortedQuotes[i].lastName = "Only";
                    }

                    unSortedFiles.Clear();
                    unSortedNotes.Clear();
                    unSortedEvents.Clear();

                    // Convert to local time
                    sortedQuotes[i].timestamp = sortedQuotes[i].timestamp.ToLocalTime();
                    sortedQuotes[i].lastUpdated = sortedQuotes[i].lastUpdated.ToLocalTime();

                    for (var j = 0; j < sortedQuotes[i].attachments.Count; j++)
                    {
                        Libraries.Models.Attachment currentAttachment = sortedQuotes[i].attachments[j];

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

        public List<Quote> GetQuotes(string status, string userId)
        {
            List<Quote> sortedQuotes = new List<Quote>();
            try
            {
                var builder = Builders<Quote>.Filter;
                var filter = Builders<Quote>.Filter.And(
                    Builders<Quote>.Filter.Where(p => p.status.Contains(status)),
                    Builders<Quote>.Filter.Where(p => p.projectManager.Email.Contains(userId))
                    );

                sortedQuotes = _mongoDBCollection.Find(filter).SortByDescending(e => e.timestamp).ToList();
                for (var i = 0; i < sortedQuotes.Count; i++)
                {
                    if (sortedQuotes[i].firstName == "")
                    {
                        sortedQuotes[i].firstName = "Address";
                    }

                    if (sortedQuotes[i].lastName == "")
                    {
                        sortedQuotes[i].lastName = "Only";
                    }

                    unSortedFiles.Clear();
                    unSortedNotes.Clear();
                    unSortedEvents.Clear();

                    // Convert to local time
                    sortedQuotes[i].timestamp = sortedQuotes[i].timestamp.ToLocalTime();
                    sortedQuotes[i].lastUpdated = sortedQuotes[i].lastUpdated.ToLocalTime();

                    for (var j = 0; j < sortedQuotes[i].attachments.Count; j++)
                    {
                        Libraries.Models.Attachment currentAttachment = sortedQuotes[i].attachments[j];

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

        public List<ListItem> GetTradesList(bool filterActiveTrade, string activeQuoteTrade)
        {
            List<ListItem> masterList = new List<ListItem>();

            try
            {
                var filter = new BsonDocument();

                var tradeFilter = Builders<Quote>.Filter.Where(p => p._t == "Quote");

                //if (filterActiveTrade)
                //{
                //    var dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.applicationType, activeQuoteTrade));

                //    tradeFilter = Builders<Quote>.Filter.And(filter, dataSourceFilter);
                //}

                var tradesList = _mongoDBCollection.Distinct(s => s.applicationType, tradeFilter).ToList();

                foreach (var trade in tradesList)
                {
                    if(!trade.Contains("Aggregate"))
                    {
                        ListItem tradeItem = new ListItem();
                        tradeItem.Text = trade;
                        tradeItem.Value = trade;

                        masterList.Add(tradeItem);
                    }
                }

                return masterList;
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }

            return null;
        }

        public List<string> GetTradesList(string status)
        {
            try
            {
                var filter = new BsonDocument();

                if (status == "All")
                {
                    var statusFilter = Builders<Quote>.Filter.Where(p => p._t == "Quote");

                    var tradesList = _mongoDBCollection.Distinct(s => s.applicationType, statusFilter);

                    return tradesList.ToList();
                }
                else
                {
                    var statusFilter = Builders<Quote>.Filter.Where(p => p.status.Contains(status));

                    var tradesList = _mongoDBCollection.Distinct(s => s.applicationType, statusFilter);

                    return tradesList.ToList();
                }
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }

            return null;
        }

        public List<Quote> GetQuote(string quoteId)
        {
            try
            {
                var builder = Builders<Quote>.Filter;
                var filter = builder.Eq(f => f._id, quoteId);

                var sortedQuotes = _mongoDBCollection.Find(filter).ToList();
                if (sortedQuotes.Count > 0)
                {
                    for (var i = 0; i < sortedQuotes.Count; i++)
                    {
                        if (sortedQuotes[i].firstName == "")
                        {
                            sortedQuotes[i].firstName = "Address";
                        }

                        if (sortedQuotes[i].lastName == "")
                        {
                            sortedQuotes[i].lastName = "Only";
                        }

                        unSortedFiles.Clear();
                        unSortedNotes.Clear();
                        unSortedEvents.Clear();

                        // Convert to local time
                        sortedQuotes[i].timestamp = sortedQuotes[i].timestamp.ToLocalTime();
                        sortedQuotes[i].lastUpdated = sortedQuotes[i].lastUpdated.ToLocalTime();

                        for (var j = 0; j < sortedQuotes[i].attachments.Count; j++)
                        {
                            Libraries.Models.Attachment currentAttachment = sortedQuotes[i].attachments[j];

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
                else
                {
                    return null;
                }
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

        IMongoCollection<Quote> _quoteCollection;

        public bool CreateQuote(Quote newQuote)
        {
            try
            {
                _quoteCollection = _mongoDBClient.GetDatabase(_mongoDBName).GetCollection<Quote>(_mongoDBCollectionName);

                _quoteCollection.InsertOne(newQuote);

                return true;
            }
            catch(Exception ex)
            {
                var errMsg = ex.Message;
                return false;
            }

            return false;
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

        public QuoteLeaderboard GetLeaderBoardStats(string status, bool showHideTestData)
        {
            QuoteLeaderboard quoteLeaderboard = new QuoteLeaderboard();

            quoteLeaderboard.Trades = GetTradesList(status);

            var searchResults = new List<QuoteStat>();
            var dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.externalUrl, "Auto-generated WebApi"));

            try
            {
                if (!showHideTestData)
                {

                    var countsByQuoteStatus = _mongoDBCollection.Aggregate()
                        .Match(dataSourceFilter)
                              .Group(
                                  x => x.status,
                                  g => new QuoteStat
                                  {
                                      QuoteType = g.Key,
                                      QuoteCount = g.Count(),
                                      QuoteValue = g.Sum(x => x.totalQuote),
                                      QuoteSqFt = g.Sum(x => x.totalSquareFeet)
                                  }
                                  ).ToList();

                    searchResults = countsByQuoteStatus;
                }
                else
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

                    searchResults = countsByQuoteStatus;
                }

                foreach (var item in searchResults)
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

        public QuoteLeaderboard GetLeaderBoardStats(string tradeType, string status, bool showHideTestData)
        {
            QuoteLeaderboard quoteLeaderboard = new QuoteLeaderboard();

            quoteLeaderboard.Trades = GetTradesList(status);

            var searchResults = new List<QuoteStat>();
            var dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.externalUrl, "Auto-generated WebApi"));

            //var tradeFilter = Builders<Quote>.Filter.Where(p => p.applicationType.Contains(utils.TitleCaseString(tradeType)));

            var filterBuilder = Builders<Quote>.Filter;
            var filter = filterBuilder.Eq("status", status) & filterBuilder.Eq("applicationType", utils.TitleCaseString(tradeType));

            if (!showHideTestData)
            {
                dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.externalUrl, "Auto-generated WebApi"));

                filter = Builders<Quote>.Filter.And(filter, dataSourceFilter);
            }

            try
            {
                if (!showHideTestData)
                {

                    var countsByQuoteStatus = _mongoDBCollection.Aggregate()
                        .Match(filter)
                              .Group(
                                  x => x.status,
                                  g => new QuoteStat
                                  {
                                      QuoteType = g.Key,
                                      QuoteCount = g.Count(),
                                      QuoteValue = g.Sum(x => x.totalQuote),
                                      QuoteSqFt = g.Sum(x => x.totalSquareFeet)
                                  }
                                  ).ToList();

                    searchResults = countsByQuoteStatus;
                }
                else
                {
                    var countsByQuoteStatus = _mongoDBCollection.Aggregate()
                        .Match(filter)
                              .Group(
                                  x => x.status,
                                  g => new QuoteStat
                                  {
                                      QuoteType = g.Key,
                                      QuoteCount = g.Count(),
                                      QuoteValue = g.Sum(x => x.totalQuote),
                                      QuoteSqFt = g.Sum(x => x.totalSquareFeet)
                                  }).ToList();

                    searchResults = countsByQuoteStatus;
                }

                foreach (var item in searchResults)
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

        public QuoteLeaderboard GetLeaderBoardStats(string userId)
        {
            QuoteLeaderboard quoteLeaderboard = new QuoteLeaderboard();

            try
            {
                var countsByQuoteStatus = _mongoDBCollection.Aggregate()
                    .Match(x => x.projectManager.Email == userId)
                          .Group(
                              x => x.status,
                              g => new QuoteStat
                              {
                                  QuoteType = g.Key,
                                  QuoteCount = g.Count(),
                                  QuoteValue = g.Sum(x => x.totalQuote),
                                  QuoteSqFt = g.Sum(x => x.totalSquareFeet)
                              }
                              ).ToList();

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
            if(originalList != null)
                return originalList.OrderByDescending(x => x.DateTime).ToList();
            else return originalList;
        }

        public List<Libraries.Models.Attachment> SortFilesByDateDesc(List<Libraries.Models.Attachment> originalList)
        {
            if (originalList != null)
                return originalList.OrderByDescending(x => x.Date).ToList();
            else return originalList;
        }

        public List<Note> SortNotesByDateDesc(List<Note> originalList)
        {
            if (originalList != null)
                return originalList.OrderByDescending(x => x.Date).ToList();
            else return originalList;
        }

        public bool SendEmail(string ownerId, string ownerType, string fromAddress, string toAddress, string subject, string body, bool isBodyHtml)
        {
            try
            {
                //Set up message
                var message = new MailMessage { From = new MailAddress(fromAddress) };
                message.To.Add(new MailAddress(toAddress));
                message.Subject = subject;
                message.IsBodyHtml = isBodyHtml;

                var messageBody = body;
                messageBody = messageBody.Replace("[DocTitle]", subject);
                messageBody = messageBody.Replace("[Date]", DateTime.UtcNow.ToLocalTime().ToString(CultureInfo.InvariantCulture));
                messageBody = messageBody.Replace("[MessageBody]", body.Replace("|", isBodyHtml ? "<br />" : Environment.NewLine));

                message.Body = messageBody;

                message.Priority = MailPriority.High;

                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                //// setup Smtp Client
                //var smtp = new SmtpClient
                //{
                //    Port = 25,
                //    Host = "email-smtp.us-east-2.amazonaws.com",
                //    EnableSsl = true,
                //    UseDefaultCredentials = true,
                //    Credentials = new NetworkCredential("admin@facilitate.org", "!Facilitate2024#"),
                //    DeliveryMethod = SmtpDeliveryMethod.Network
                //};

                //smtp.Send(message);

                return true;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex)
            {
                var errMsg = ex.Message;
            }
            return false;
        }
    }
}
