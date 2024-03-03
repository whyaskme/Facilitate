using Facilitate.Libraries.Models;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Facilitate.Libraries.Models.Constants.Transaction;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Facilitate.Libraries.Models
{
    public class Quote : Base
    {
        public Quote()
        {
            _t = "Quote";
            _id = ObjectId.GenerateNewId();

            Name = _t;

            SessionId = string.Empty;
            LeadId = string.Empty;
            Timestamp = DateTime.Now;
            ExternalUrl = string.Empty;

            Consumer = new User();
            PropertyInfo = new PropertyInfo();
            Products = new List<Product>();
            Representative = new Representative();
        }

        public string SessionId { get; set; }
        public string LeadId { get; set; }
        public DateTime Timestamp { get; set; }
        public string ExternalUrl { get; set; }

        public User Consumer { get; set; }
        public PropertyInfo PropertyInfo { get; set; }
        public List<Product> Products { get; set; }
        public Representative Representative { get; set; }

        #region Implementation of IEnumerable
        List<Quote> quotes;
        public IEnumerator<Quote> GetEnumerator()
        {
            return quotes.GetEnumerator();
        }
        #endregion
    }

}
