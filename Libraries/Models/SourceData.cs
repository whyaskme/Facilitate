﻿using System;

using MongoDB.Bson;

namespace Facilitate.Libraries.Models
{
    public class SourceData : Base
    {
        public SourceData()
        {
            _t = "SourceData";

            Zip = 00000;
            City = string.Empty;
            CityId = ObjectId.Empty;
            StateAbbr = string.Empty;
            StateId = ObjectId.Empty;
            County = string.Empty;
            CountyId = ObjectId.Empty;
            TimeZone = string.Empty;
            TimeZoneId = ObjectId.Empty;
            AreaCodes = string.Empty;
            Latitude = 00.00;
            Longitude = 00.00;
            Country = string.Empty;
            CountryId = ObjectId.Empty;
            EstimatedPopulation = 0;
        }

        public Int32 Zip { get; set; }
        public string City { get; set; }
        public ObjectId CityId { get; set; }
        public string StateAbbr { get; set; }
        public ObjectId StateId { get; set; }
        public string County { get; set; }
        public ObjectId CountyId { get; set; }
        public string TimeZone { get; set; }
        public ObjectId TimeZoneId { get; set; }
        public string AreaCodes { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
        public string Country { get; set; }
        public ObjectId CountryId { get; set; }
        public Int32 EstimatedPopulation { get; set; }
    }
}