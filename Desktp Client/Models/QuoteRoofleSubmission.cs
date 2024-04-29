using System;
using System.Collections.Generic;

namespace Facilitate.Desktop.Models
{
    public class QuoteRoofleSubmission
    {
        public QuoteRoofleSubmission()
        {
            address = "123 My Street";
            fullAddress = "123 My Street Austin, Tx 78753";
            street = "123 My Street";
            city = "Austin";
            state = "Tx";
            zip = "78753";
            firstName = "John";
            lastName = "Tester";
            email = "john@tester.com";
            phone = "555-555-1212";
            market = "Austin";
            externalUrl = "Roofle.com";
            timestamp = DateTime.UtcNow;
            numberOfStructures = 1;
            numberOfIncludedStructures = 1;
            totalSquareFeet = 1475;
            mainRoofTotalSquareFeet = 1475;
            totalInitialSquareFeet = 1475;
            sessionId = "87go^%DUssye56536";
            structures = new List<Structure>();
            repLead = "Marc Spitz";
            repEmail = "marc@spitz.com";
            leadId = 0;
            products = new List<Product>();
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

        public string repLead { get; set; }
        public string repEmail { get; set; }
        public int leadId { get; set; }

        public List<Product> products { get; set; }
        public List<Structure> structures { get; set; }

        public int mainRoofTotalSquareFeet { get; set; }
        public int totalInitialSquareFeet { get; set; }
        public string sessionId { get; set; }
    }

}
