﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class QuoteHeader
    {
        public QuoteHeader()
        {
            _id = string.Empty;
            _t = "QuoteHeader";
            rowIndex = 0;
            isQualified = false;
            Trade = string.Empty;
            TradeCategory = string.Empty;
            status = string.Empty;
            street = string.Empty;
            city = string.Empty;
            state = string.Empty;
            zip = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            email = string.Empty;
            relationship = string.Empty;
            numberOfStructures = 0;
            numberOfIncludedStructures = 0;
            totalSquareFeet = 0;
            totalQuote = 0;
            timestamp = DateTime.UtcNow;

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
        public string TradeCategory { get; set; }
        public string status { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string relationship { get; set; }
        public int numberOfStructures { get; set; }
        public int numberOfIncludedStructures { get; set; }
        public int totalSquareFeet { get; set; }
        public double totalQuote { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime lastUpdated { get; set; }

        public virtual List<Event>? events { get; set; }

        public virtual ApplicationUser? Bidder { get; set; }
        public string BidderType { get; set; }
        public DateTime BiddingExpires { get; set; }

    }
}
