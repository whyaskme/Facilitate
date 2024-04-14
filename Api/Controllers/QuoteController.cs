using Facilitate.Libraries.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using Api.Controllers;
using static Facilitate.Libraries.Models.Constants.Messaging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Facilitate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        Utils utils = new Utils();

        string dbName = "Facilitate";
        string collectionName = "Quote";

        string resultMsg = string.Empty;

        //string mongoUri = "mongodb+srv://facilitate:!13324BossWood@facilitate.73z1cne.mongodb.net/?retryWrites=true&w=majority&appName=Facilitate";
        //mongodb+srv://elite-io:!113324BossWood@cluster0.wluzv.mongodb.net/DriveSwitch?replicaSet=atlas-sqh0hv-shard-0&amp;readPreference=primary&amp;connectTimeoutMS=10000&amp;authSource=admin&amp;authMechanism=SCRAM-SHA-1
        string mongoUri = "mongodb://localhost:27017/?retryWrites=true&w=majority&appName=Facilitate";

        IMongoClient client;

        IMongoCollection<Quote> collection;
        private CancellationToken _cancellationToken;

        List<Attachment> unSortedFiles = new List<Attachment>();
        List<Note> unSortedNotes = new List<Note>();
        List<Event> unSortedEvents = new List<Event>();

        private readonly ILogger<QuoteController> _logger;

        public QuoteController(ILogger<QuoteController> logger)
        {
            _logger = logger;
        }

        // GET: api/<QuoteController>
        [HttpGet]
        public IEnumerable<Quote> Get(string status)
        {
            status = utils.TitleCaseString(status);

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

        // GET api/<QuoteController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<QuoteController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<QuoteController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<QuoteController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private List<Event> SortEventsByDateDesc(List<Event> originalList)
        {
            return originalList.OrderByDescending(x => x.DateTime).ToList();
        }

        private List<Attachment> SortFilesByDateDesc(List<Attachment> originalList)
        {
            return originalList.OrderByDescending(x => x.Date).ToList();
        }

        private List<Note> SortNotesByDateDesc(List<Note> originalList)
        {
            return originalList.OrderByDescending(x => x.Date).ToList();
        }
    }
}
