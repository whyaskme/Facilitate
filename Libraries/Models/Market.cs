using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

using MongoDB;
using MongoDB.Bson;
//using MongoDB.Driver;
//using MongoDB.Driver.Builders;

namespace Facilitate.Libraries.Models
{
    public class Market// : Base
    {
        public Market()
        {
            //_t = "Market";

            DatePublished = DateTime.UtcNow;
            DateModified = DateTime.UtcNow;

            PublishState = "Unpublished";

            Name = "";
            Url = "http://";
            StateId = ObjectId.Empty;
        }

        public DateTime DatePublished { get; set; }
        public DateTime DateModified { get; set; }

        public string PublishState { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }
        public ObjectId StateId { get; set; }
    }
}
