using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

using Facilitate.Libraries.Models;

namespace Facilitate.Libraries.Models
{
    public class QuoteLeaderboard
    {
        public QuoteLeaderboard()
        {
            LeadCount = 0;
            LeadSqFt = 0;
            LeadValue = 0;
            LeadLabel = "Leads";

            OpportunityCount = 0;
            OpportunitySqFt = 0;
            OpportunityValue = 0;

            CustomerCount = 0;
            CustomerSqFt = 0;
            CustomerValue = 0;

            CompletionCount = 0;
            CompletionSqFt = 0;
            CompletionValue = 0;

            ArchiveCount = 0;
            ArchiveSqFt = 0;
            ArchiveValue = 0;

            WarrantyCount = 0;
            WarrantySqFt = 0;
            WarrantyValue = 0;

            TotalQuoteCount = 0;
            TotalQuoteSqFt = 0;
            TotalQuoteValue = 0;
        }

        public long LeadCount { get; set; }
        public long LeadSqFt { get; set; }
        public double LeadValue { get; set; }
        public string LeadLabel { get; set; }

        public long OpportunityCount { get; set; }
        public long OpportunitySqFt { get; set; }
        public double OpportunityValue { get; set; }


        public long CustomerCount { get; set; }
        public long CustomerSqFt { get; set; }
        public double CustomerValue { get; set; }


        public long CompletionCount { get; set; }
        public long CompletionSqFt { get; set; }
        public double CompletionValue { get; set; }


        public long ArchiveCount { get; set; }
        public long ArchiveSqFt { get; set; }
        public double ArchiveValue { get; set; }


        public long WarrantyCount { get; set; }
        public long WarrantySqFt { get; set; }
        public double WarrantyValue { get; set; }

        public long TotalQuoteCount { get; set; }
        public long TotalQuoteSqFt { get; set; }
        public double TotalQuoteValue { get; set; }
    }
}
