using System;
using System.Collections;
using System.Collections.Generic;
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
    public class TimeZone : Base
    {
        public TimeZone()
        {
            _t = "TimeZone";

            Region = string.Empty;
        }
        public string Region { get; set; }
    }
}