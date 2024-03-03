using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Libraries.Models;
using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class Product
    {
        public string? Name { get; set; }
        public string? Id { get; set; }
        public string? WasteFactorMainRoof { get; set; }

        public PriceInfo? PriceInfo { get; set; }
        public PriceRange? PriceRange { get; set; }
    }
}
