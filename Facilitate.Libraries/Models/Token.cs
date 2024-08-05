using System;
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
    public class Token
    {
        public Token()
        {
            _id = ObjectId.GenerateNewId();
            Value = string.Empty;
        }

        public ObjectId _id { get; set; }
        public string Value { get; set; }
    }
}
