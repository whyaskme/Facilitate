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
using MongoDB.Driver;
//using MongoDB.Driver.Builders;

namespace Facilitate.Libraries.Models
{
    public class Stats
    {
        public Stats()
        {
            _id = ObjectId.GenerateNewId();
            _t = "Stats";

            Users = 0;
            MarketsAssigned = 0;
            MarketsUnAssigned = 0;
            GmailRegistered = 0;
            GmailUnRegistered = 0;
            CraigslistRegistered = 0;
            CraigslistUnRegistered = 0;
        }

        public ObjectId _id { get; set; }
        public string _t { get; set; }

        public Int64 Users { get; set; }
        public Int64 MarketsAssigned { get; set; }
        public Int64 MarketsUnAssigned { get; set; }
        public Int64 GmailRegistered { get; set; }
        public Int64 GmailUnRegistered { get; set; }
        public Int64 CraigslistRegistered { get; set; }
        public Int64 CraigslistUnRegistered { get; set; }
    }
}
