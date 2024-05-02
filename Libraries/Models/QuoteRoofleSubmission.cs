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
    public class QuoteRoofleSubmission
    {
        public QuoteRoofleSubmission()
        {
            //address = string.Empty;
            //fullAddress = string.Empty;
            //street = string.Empty;
            //city = string.Empty;
            //state = string.Empty;
            //zip = string.Empty;
            //firstName = string.Empty;
            //lastName = string.Empty;
            //email = string.Empty;
            //phone = string.Empty;
            //market = string.Empty;
            //externalUrl = string.Empty;
            //timestamp = DateTime.UtcNow;
            //numberOfStructures = 0;
            //numberOfIncludedStructures = 0;
            //totalSquareFeet = 0;
            //mainRoofTotalSquareFeet = 0;
            //totalInitialSquareFeet = 0;
            //sessionId = string.Empty;
            //structures = new List<Structure>();
            //repName = string.Empty;
            //repEmail = string.Empty;
            //leadId = 0;
            //products = new List<Product>();
        }

        public string address { get; set; }
        public string fullAddress { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string market { get; set; }
        public string externalUrl { get; set; }
        public DateTime timestamp { get; set; }

        public int numberOfStructures { get; set; }
        public int numberOfIncludedStructures { get; set; }
        public int totalSquareFeet { get; set; }

        public string repName { get; set; }
        public string repEmail { get; set; }
        public int leadId { get; set; }

        public List<Product>? products { get; set; }
        public List<Structure>? structures { get; set; }

        public int mainRoofTotalSquareFeet { get; set; }
        public int totalInitialSquareFeet { get; set; }
        public string sessionId { get; set; }
    }

}
