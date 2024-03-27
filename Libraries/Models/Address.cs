using System;
using System.Collections.Generic;
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
            CountryId = Constants.DefaultCountryId;
            Country = string.Empty;

            StateId = ObjectId.Empty;
            State = string.Empty;

            CountyId = ObjectId.Empty;
            County = string.Empty;

            CityId = ObjectId.Empty;
            City = string.Empty;

            ZipCode = 00000;

            TimeZoneId = ObjectId.Empty;
            TimeZone = string.Empty;

            Address1 = string.Empty;
            Address2 = string.Empty;
        }

        public ObjectId CountryId { get; set; }
        public string Country { get; set; }

        public ObjectId StateId { get; set; }
        public string State { get; set; }

        public ObjectId CountyId { get; set; }
        public string County { get; set; }

        public ObjectId CityId { get; set; }
        public string City { get; set; }

        public Int32 ZipCode { get; set; }

        public ObjectId TimeZoneId { get; set; }
        public string TimeZone { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
}