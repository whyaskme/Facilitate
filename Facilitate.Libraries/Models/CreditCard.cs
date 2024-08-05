using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class CreditCard
    {
        public CreditCard()
        {
            _id = ObjectId.GenerateNewId();
            _t = "CreditCard";

            FullName = string.Empty;
            CardTypeId = ObjectId.Empty;
            CardTypeName = string.Empty;
            Number = string.Empty;
            Expires = string.Empty;
            Zipcode = string.Empty;
            CVVCode = string.Empty;
        }

        public ObjectId _id { get; set; }
        public string _t { get; set; }
        public String FullName { get; set; }
        public ObjectId CardTypeId { get; set; }
        public String CardTypeName { get; set; }
        public String Number { get; set; }
        public String Expires { get; set; }
        public String Zipcode { get; set; }
        public String CVVCode { get; set; }
    }
}