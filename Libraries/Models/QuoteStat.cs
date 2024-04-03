using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class QuoteStat
    {
        public long LeadCount { get; set; }
        public long OpportunityCount { get; set; }
        public long CustomerCount { get; set; }
        public long CompletionCount { get; set; }
        public long ArchiveCount { get; set; }
        public long WarrantyCount { get; set; }
    }
}
