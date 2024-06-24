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
            _id = ObjectId.GenerateNewId().ToString();
            _t = "Warranty";

            DateCreated = DateTime.UtcNow;

            isCompleted = false;
            //DateCompleted = DateTime.MinValue;
            //CompletionDetails = string.Empty;

            Summary = string.Empty;
            Details = string.Empty;

            Author = new ApplicationUser();
        }


        public string _id { get; set; }
        public string _t { get; set; }

        public DateTime? DateCreated { get; set; }

        public bool isCompleted { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string CompletionDetails { get; set; }

        public string Summary { get; set; }
        public string Details { get; set; }
        public ApplicationUser Author { get; set; }

    }
}
