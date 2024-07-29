using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class Event
    {
        public Event()
        {
            _id = ObjectId.GenerateNewId().ToString();
            _t = "Event";

            Trade = string.Empty;
            Name = string.Empty;
            Relationship = string.Empty;
            TypeId = 0;
            DateTime = DateTime.UtcNow;
            Details = "None";
            Author = new ApplicationUser();
        }

        public string _id { get; set; }
        public string _t { get; set; }
        public string Trade { get; set; }
        public string Name { get; set; }
        public string Relationship { get; set; }
        public int TypeId { get; set; }
        public DateTime DateTime { get; set; }
        public string Details { get; set; }
        public ApplicationUser Author { get; set; }
    }
}