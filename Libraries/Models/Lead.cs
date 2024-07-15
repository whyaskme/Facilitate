using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class Lead
    {
        public string _id { get; set; }
        public string _t { get; set; }
        public string applicationType { get; set; }
        public string status { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string externalUrl { get; set; }
        public int totalSquareFeet { get; set; }
        public double totalQuote { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime lastUpdated { get; set; }

        //public virtual ApplicationUser? projectManager { get; set; }
    }
}
