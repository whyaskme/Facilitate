﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
//using MongoDB.Driver.Builders;

namespace Facilitate.Libraries.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ZipCode : Base
    {
        /// <summary>
        /// 
        /// </summary>
        public ZipCode()
        {
            _t = "Zip";

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

        /// <summary>
        /// 
        /// </summary>
        public Int32 Zip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObjectId CityId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StateAbbr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObjectId StateId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string County { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObjectId CountyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TimeZone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObjectId TimeZoneId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string AreaCodes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Double Latitude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Double Longitude { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObjectId CountryId { get; set; }
        public Int32 EstimatedPopulation { get; set; }
    }
}
