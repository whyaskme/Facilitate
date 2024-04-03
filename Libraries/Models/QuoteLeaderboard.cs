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
        public int LeadSqFt { get; set; }
        public double LeadValue { get; set; }
        

        public long OpportunityCount { get; set; }
        public int OpportunitySqFt { get; set; }
        public double OpportunityValue { get; set; }


        public long CustomerCount { get; set; }
        public int CustomerSqFt { get; set; }
        public double CustomerValue { get; set; }


        public long CompletionCount { get; set; }
        public int CompletionSqFt { get; set; }
        public double CompletionValue { get; set; }


        public long ArchiveCount { get; set; }
        public int ArchiveSqFt { get; set; }
        public double ArchiveValue { get; set; }


        public long WarrantyCount { get; set; }
        public int WarrantySqFt { get; set; }
        public double WarrantyValue { get; set; }
    }
}
