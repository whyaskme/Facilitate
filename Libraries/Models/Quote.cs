using Facilitate.Libraries.Models;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Facilitate.Libraries.Models.Constants.Transaction;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Facilitate.Libraries.Models
{
    public class Quote
    {
        public Quote()
        {
            this.Structures = new List<Structure>();
            this.Products = new List<Product>();    
            //this.Date = new  DateTime.Date.;
            this.Address = string.Empty;
            this.Street = string.Empty;
            this.NumberOfStructures = string.Empty;
            this.RepLead = string.Empty;
            this.Phone = string.Empty;
        }

        public DateAndTime? Date { get; set; }

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
        public string NumberOfStructures { get; set; }
        public string NumberOfIncludedStructures { get; set; }
        public string TotalSquareFeet { get; set; }
        public string RepLead { get; set; }
        public string RepEmail { get; set; }
        public string LeadId { get; set; }

        public string MainRoofTotalSquareFeet { get; set; }
        public string TotalInitialSquareFeet { get; set; }
        public string SessionId { get; set; }

        public List<Product> Products { get; set; }
        public List<Structure> Structures { get; set; }
    }
}
