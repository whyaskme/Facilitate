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

        public List<ListItem> GetColumnWidths()
        {
            List<ListItem> columnWidths = new List<ListItem>();

            ListItem columItem = new ListItem();
            columItem.Text = "Row";
            columItem.Value = "35";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Record Id";
            columItem.Value = "215";
            columnWidths.Add(columItem);
            
            columItem = new ListItem();
            columItem.Text = "Status";
            columItem.Value = "100";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "IsQualified";
            columItem.Value = "85";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Trade";
            columItem.Value = "85";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Category";
            columItem.Value = "85";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Created";
            columItem.Value = "160";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Updated";
            columItem.Value = "160";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "LastEventDetails";
            columItem.Value = "auto";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "FirstName";
            columItem.Value = "85";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "LastName";
            columItem.Value = "100";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Email";
            columItem.Value = "150";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Street";
            columItem.Value = "auto";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "City";
            columItem.Value = "125";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "State";
            columItem.Value = "50";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Zip";
            columItem.Value = "65";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "SqFt";
            columItem.Value = "65";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Bid";
            columItem.Value = "85";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Bidder";
            columItem.Value = "125";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "BidExpires";
            columItem.Value = "100";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "BidType";
            columItem.Value = "100";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Actions";
            columItem.Value = "100";
            columnWidths.Add(columItem);

            columItem = new ListItem();
            columItem.Text = "Relationship";
            columItem.Value = "100";
            columnWidths.Add(columItem);

            return columnWidths;
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
                    .Include(p => p.Bidder)
                    .Include(p => p.BidderType)
                    .Include(p => p.BiddingExpires)
                    .Include(p => p.Trade)
                    .Include(p => p.TradeCategory)
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

                var rowIndex = quoteHeaders.Count;

                foreach (var quote in quoteHeaders)
                {
                    rowIndex--;

                    //quote.rowIndex = rowIndex;
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
                    .Include(p => p.Bidder)
                    .Include(p => p.Trade)
                    .Include(p => p.TradeCategory)
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
                var filter = filterBuilder.Eq("status", status) & filterBuilder.Eq("Trade", utils.TitleCaseString(tradeType));

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
                    .Include(p => p.Trade)
                    .Include(p => p.TradeCategory)
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
                    Builders<Quote>.Filter.Where(p => p.Bidder.Email.Contains(userId))
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

        public List<QuoteSummary> GetRelatedQuoteSummaries(Quote quote)
        {
            List<QuoteSummary> QuoteHeaders = new List<QuoteSummary>();

            try
            {
                foreach(var relationship in quote.relationships)
                {
                    var filter = Builders<Quote>.Filter.Eq(f => f._id, relationship._id);

                    Quote _quote = _mongoDBCollection.Find(filter).ToList()[0];

                    var rowIndex = 0;

                    QuoteSummary _summary = new QuoteSummary();

                    rowIndex++;

                    _summary.rowIndex = rowIndex;

                    var quoteId = relationship._id;
                    var filter2 = Builders<Quote>.Filter.Eq(f => f._id, quoteId);
                    var relatedQuote = _mongoDBCollection.Find(filter2).ToList()[0];

                    _summary.relationship = relationship.Type;
                    _summary._id = relatedQuote._id;
                    _summary.Trade = relatedQuote.Trade;
                    _summary.status = relatedQuote.status;

                    _summary.totalQuote = relatedQuote.totalQuote;

                    _summary.Bidder = relatedQuote.Bidder;
                    _summary.BidderType = relatedQuote.BidderType;
                    _summary.BiddingExpires = relatedQuote.BiddingExpires;

                    if(relatedQuote.events.Count > 0)
                    {
                        Event lastEvent = relatedQuote.events.LastOrDefault();
                        _summary.events = relatedQuote.events;
                        _summary.lastEventDetails = lastEvent.Details;
                        _summary.lastEventTimeStamp = lastEvent.DateTime.ToLocalTime();
                    }

                    QuoteHeaders.Add(_summary);
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

                        var currentDateTime = currentAttachment.DateTime;

                        currentAttachment.DateTime = currentAttachment.DateTime.ToLocalTime();

                        unSortedFiles.Add(currentAttachment);
                    }

                    for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
                    {
                        Note currentNote = sortedQuotes[i].notes[j];
                        currentNote.DateTime = currentNote.DateTime.ToLocalTime();

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
                var tradeFilter = Builders<Quote>.Filter.Where(p => p.Trade.Contains(utils.TitleCaseString(tradeType)));

                var filterBuilder = Builders<Quote>.Filter;
                var filter = filterBuilder.Eq("status", status) & filterBuilder.Eq("Trade", utils.TitleCaseString(tradeType));

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

                        var currentDateTime = currentAttachment.DateTime;

                        currentAttachment.DateTime = currentAttachment.DateTime.ToLocalTime();

                        unSortedFiles.Add(currentAttachment);
                    }

                    for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
                    {
                        Note currentNote = sortedQuotes[i].notes[j];
                        currentNote.DateTime = currentNote.DateTime.ToLocalTime();

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

                        var currentDateTime = currentAttachment.DateTime;

                        currentAttachment.DateTime = currentAttachment.DateTime.ToLocalTime();

                        unSortedFiles.Add(currentAttachment);
                    }

                    for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
                    {
                        Note currentNote = sortedQuotes[i].notes[j];
                        currentNote.DateTime = currentNote.DateTime.ToLocalTime();

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
                    Builders<Quote>.Filter.Where(p => p.Bidder.Email.Contains(userId))
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

                        var currentDateTime = currentAttachment.DateTime;

                        currentAttachment.DateTime = currentAttachment.DateTime.ToLocalTime();

                        unSortedFiles.Add(currentAttachment);
                    }

                    for (var j = 0; j < sortedQuotes[i].notes.Count; j++)
                    {
                        Note currentNote = sortedQuotes[i].notes[j];
                        currentNote.DateTime = currentNote.DateTime.ToLocalTime();

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
                //    var dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.Trade, activeQuoteTrade));

                //    tradeFilter = Builders<Quote>.Filter.And(filter, dataSourceFilter);
                //}

                var tradesList = _mongoDBCollection.Distinct(s => s.Trade, tradeFilter).ToList();

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

                    var tradesList = _mongoDBCollection.Distinct(s => s.Trade, statusFilter);

                    return tradesList.ToList();
                }
                else
                {
                    var statusFilter = Builders<Quote>.Filter.Where(p => p.status.Contains(status));

                    var tradesList = _mongoDBCollection.Distinct(s => s.Trade, statusFilter);

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

                var quoteList = _mongoDBCollection.Find(filter).ToList();

                return quoteList;
            }
            catch (Exception ex)
            {
                return null;
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

        [HttpPut("{quote}")]
        public Quote DeleteQuote(Quote quote)
        {
            string quoteId = quote._id;

            try
            {
                var filter = Builders<Quote>.Filter.Eq(x => x._id, quoteId);

                var result = _mongoDBCollection.DeleteOne(filter, _cancellationToken);
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

        public QuoteLeaderboard GetLeaderboardStats()
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

        public QuoteLeaderboard GetLeaderboardStats(string status, bool showHideTestData)
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

        public QuoteLeaderboard GetLeaderboardStats(string tradeType, string status, bool showHideTestData)
        {
            QuoteLeaderboard quoteLeaderboard = new QuoteLeaderboard();

            quoteLeaderboard.Trades = GetTradesList(status);

            var searchResults = new List<QuoteStat>();
            var dataSourceFilter = Builders<Quote>.Filter.Not(Builders<Quote>.Filter.Eq(p => p.externalUrl, "Auto-generated WebApi"));

            //var tradeFilter = Builders<Quote>.Filter.Where(p => p.Trade.Contains(utils.TitleCaseString(tradeType)));

            var filterBuilder = Builders<Quote>.Filter;
            var filter = filterBuilder.Eq("status", status) & filterBuilder.Eq("Trade", utils.TitleCaseString(tradeType));

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

        public QuoteLeaderboard GetLeaderboardStats(string userId)
        {
            QuoteLeaderboard quoteLeaderboard = new QuoteLeaderboard();

            try
            {
                var countsByQuoteStatus = _mongoDBCollection.Aggregate()
                    .Match(x => x.Bidder.Email == userId)
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
                return originalList.OrderByDescending(x => x.DateTime).ToList();
            else return originalList;
        }

        public List<Note> SortNotesByDateDesc(List<Note> originalList)
        {
            if (originalList != null)
                return originalList.OrderByDescending(x => x.DateTime).ToList();
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
