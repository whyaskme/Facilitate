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
            _id = string.Empty;
            _t = "QuoteSummary";
            isQualified = true;
            applicationType = string.Empty;
            relationship = string.Empty;
            status = string.Empty;

            totalQuote = 0;
            timestamp = DateTime.UtcNow;
            lastUpdated = DateTime.UtcNow;
            events = new List<Event>();

            projectManager = null;
        }

        public string _id { get; set; }
        public string _t { get; set; }
        public bool isQualified { get; set; }
        public string applicationType { get; set; }
        public string relationship { get; set; }
        public string status { get; set; }
        
        public double totalQuote { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime lastUpdated { get; set; }
        public List<Event> events { get; set; }

        public virtual ApplicationUser? projectManager { get; set; }
    }
}
