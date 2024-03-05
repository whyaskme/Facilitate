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

            Address = string.Empty;
            FullAddress = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Phone = string.Empty;
            Market = string.Empty;
            ExternalUrl = string.Empty;
            Timestamp = string.Empty;
            NumberOfStructures = 0;
            NumberOfIncludedStructures = 0;
            TotalSquareFeet = 0;
            RepLead = string.Empty;
            RepEmail = string.Empty;
            LeadId = 0;

            Products = new List<Product>();
            Structures = new List<Structure>(); 

            MainRoofTotalSquareFeet = string.Empty;
            TotalInitialSquareFeet = string.Empty;
    }

        //public string _t { get; set; }
        public string Address { get; set; }
        public string FullAddress { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Market { get; set; }
        public string ExternalUrl { get; set; }
        public string Timestamp { get; set; }
        public int NumberOfStructures { get; set; }
        public int NumberOfIncludedStructures { get; set; }
        public int TotalSquareFeet { get; set; }
        public string RepLead { get; set; }
        public string RepEmail { get; set; }
        public int LeadId { get; set; }

        public List<Product> Products { get; set; }
        public List<Structure> Structures { get; set; }

        public string MainRoofTotalSquareFeet { get; set; }
        public string TotalInitialSquareFeet { get; set; }
        public string SessionId { get; set; }

        #region Implementation of IEnumerable
        List<Quote> quotes;
        public IEnumerator<Quote> GetEnumerator()
        {
            return quotes.GetEnumerator();
        }
        #endregion
    }

}
