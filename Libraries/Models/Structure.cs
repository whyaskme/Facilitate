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
            Name = string.Empty;
            Slope = string.Empty;
            IsIncluded = false;
            SquareFeet = 0;
            InitialSquareFeet = 0;
            RoofComplexity = string.Empty;
        }

        public string Name { get; set; }
        public string Slope { get; set; }
        public bool IsIncluded { get; set; }
        public int SquareFeet { get; set; }
        public int InitialSquareFeet { get; set; }
        public string RoofComplexity { get; set; }
    }
}
