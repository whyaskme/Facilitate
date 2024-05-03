using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class Address
    {
        public Address()
        {
            Country = string.Empty;
            State = string.Empty;
            County = string.Empty;
            City = string.Empty;
            ZipCode = string.Empty;
            TimeZone = string.Empty;
            Address1 = string.Empty;
            Address2 = string.Empty;
        }

        [Key]
        public string UserId { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string TimeZone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
}