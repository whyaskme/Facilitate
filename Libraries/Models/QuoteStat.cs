using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class QuoteStat
    {
        public string QuoteType { get; set; }
        public long QuoteCount { get; set; }
        public double QuoteValue { get; set; }
        public long QuoteSqFt { get; set; }
    }
}
