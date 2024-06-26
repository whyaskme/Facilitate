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

namespace Facilitate.Libraries.Models
{
    public class City : Base
    {
        public City()
        {
            _t = "City";

            CountryId = ObjectId.Empty;
            TimeZoneId = ObjectId.Empty;
            StateId = ObjectId.Empty;
            CountyId = ObjectId.Empty;
        }
        public ObjectId CountryId { get; set; }
        public ObjectId StateId { get; set; }
        public ObjectId CountyId { get; set; }
        public ObjectId TimeZoneId { get; set; }
        public Int32 EstimatedPopulation { get; set; }
    }
}
