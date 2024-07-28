using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class QuoteSummary
    {
        public QuoteSummary()
        {
            _id = ObjectId.GenerateNewId().ToString();
            _t = "QuoteSummary";

            rowIndex = 0;

            isQualified = false;
            Trade = string.Empty;
            TradeSubcategory = string.Empty;
            relationship = string.Empty;
            status = string.Empty;
            actions = string.Empty;

            totalQuote = 0;
            timestamp = DateTime.UtcNow;
            lastEventTimeStamp = DateTime.UtcNow;
            lastEventDetails = string.Empty;
            events = new List<Event>();

            Bidder = null;
            BidderType = "Open";
            BiddingExpires = DateTime.UtcNow;
        }

        public string _id { get; set; }
        public string _t { get; set; }
        public int rowIndex { get; set; }
        public bool isQualified { get; set; }
        public string Trade { get; set; }
        public string TradeSubcategory { get; set; }
        public string relationship { get; set; }
        public string status { get; set; }
        public string actions { get; set; }

        public double totalQuote { get; set; }
        public DateTime timestamp { get; set; }
        public List<Event> events { get; set; }
        public DateTime lastEventTimeStamp { get; set; }
        public string lastEventDetails { get; set; }

        public virtual ApplicationUser? Bidder { get; set; }
        public string BidderType { get; set; }
        public DateTime BiddingExpires { get; set; }
    }
}
