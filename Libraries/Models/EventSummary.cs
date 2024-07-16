using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class EventSummary
    {
        public EventSummary()
        {
            _id = ObjectId.GenerateNewId().ToString();
            _t = "EventSummary";

            Name = string.Empty;
            DateCreated = DateTime.UtcNow;
            Details = string.Empty;
            Author = string.Empty;
        }

        public string _id { get; set; }
        public string _t { get; set; }

        public string Author { get; set; }
        public string Trade { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
