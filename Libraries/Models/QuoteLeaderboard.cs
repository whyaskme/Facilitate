using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class QuoteLeaderboard
    {
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
