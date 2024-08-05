using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class Structure
    {
        public Structure()
        {
            name = string.Empty;
            slope = string.Empty;
            isIncluded = false;
            squareFeet = 0;
            initialSquareFeet = 0;
            roofComplexity = string.Empty;
        }

        public string name { get; set; }
        public string slope { get; set; }
        public bool isIncluded { get; set; }
        public int squareFeet { get; set; }
        public int initialSquareFeet { get; set; }
        public string roofComplexity { get; set; }
    }
}
