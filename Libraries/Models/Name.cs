using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilitate.Libraries.Models
{
    public class Name
    {
        //public Name()
        //{
        //    Title = string.Empty;
        //    FirstName = string.Empty;
        //    LastName = string.Empty;
        //    Suffix = string.Empty;
        //}

        //public string Title { get; set; }

        //public string FirstName { get; set; }
        //public string LastName { get; set; }

        //public string Suffix { get; set; }

        public Name()
        {
            _id = ObjectId.GenerateNewId();
            _t = "Name";
            NameValue = string.Empty;
        }
        public ObjectId _id { get; set; }
        public string _t { get; set; }
        public string NameValue { get; set; }
    }
}