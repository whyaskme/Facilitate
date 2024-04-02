using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Facilitate.Libraries.Models
{
    public class Warranty
    {
        public Warranty()
        {
            id = ObjectId.GenerateNewId();
            Type = string.Empty;
            
            DateCreated = DateTime.UtcNow;

            isCompleted = false;
            DateCompleted = DateTime.MinValue;

            Summary = string.Empty;
            Details = string.Empty;
        }

        
        public ObjectId id { get; set; }

        public string Type { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool isCompleted { get; set; }
        public DateTime? DateCompleted { get; set; }

        public string Summary { get; set; }
        public string Details { get; set; }

    }
}
