using System;
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
            city = string.Empty;
            state = string.Empty;
            zip = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            timestamp = DateTime.Now;
        }

        public string _id { get; set; }
        public string _t { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime timestamp { get; set; }
    }
}
