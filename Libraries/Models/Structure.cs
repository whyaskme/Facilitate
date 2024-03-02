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
        public string Name { get; set; }
        public string Slope { get; set; }
        public string IsIncluded { get; set; }
        public string SquareFeet { get; set; }
        public string InitialSquareFeet { get; set; }
        public string RoofComplexity { get; set; }
    }
}
