﻿using MongoDB.Bson;

namespace Facilitate.Libraries.Models
{
    public class Quote// : Base
    {
        public Quote()
        {
            _id = ObjectId.GenerateNewId().ToString();
            _t = "Quote";
            applicationType = "Unspecified";

            ipAddress = "127.0.0.1";
            status = "New";

            Groups = new List<ObjectId>();

            address = string.Empty;
            fullAddress = string.Empty;
            street = string.Empty;
            city = string.Empty;
            state = string.Empty;
            zip = string.Empty;
            firstName = string.Empty;
            lastName = string.Empty;
            email = string.Empty;
            phone = string.Empty;
            market = string.Empty;
            externalUrl = string.Empty;
            timestamp = DateTime.UtcNow;

            lastUpdated = DateTime.UtcNow;
            totalQuote = 0;

            numberOfStructures = 0;
            numberOfIncludedStructures = 0;
            totalSquareFeet = 0;
            mainRoofTotalSquareFeet = 0;
            totalInitialSquareFeet = 0;
            sessionId = string.Empty;
            structures = new List<Structure>();
            repName = string.Empty;
            repEmail = string.Empty;
            leadId = 0;
            relationships = new List<ListItem>();
            products = new List<Product>();
            attachments = new List<Attachment>();
            notes = new List<Note>();
            projectManager = null;
            events = new List<Event>();
            warranties = new List<Warranty>();
    }

        public string _id { get; set; }
        public string _t { get; set; }
        public string applicationType { get; set; }
        public string ipAddress { get; set; }
        public string status { get; set; }

        public List<ObjectId> Groups { get; set; }

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

        public DateTime lastUpdated { get; set; }
        public double totalQuote { get; set; }

        public int numberOfStructures { get; set; }
        public int numberOfIncludedStructures { get; set; }
        public int totalSquareFeet { get; set; }
        public int mainRoofTotalSquareFeet { get; set; }
        public int totalInitialSquareFeet { get; set; }
        public string sessionId { get; set; }

        public List<Structure>? structures { get; set; }
        public string repName { get; set; }
        public string repEmail { get; set; }
        public int leadId { get; set; }

        //public virtual User? projectManager { get; set; }
        public virtual ApplicationUser? projectManager { get; set; }

        public List<ListItem>? relationships { get; set; }
        public List<Product>? products { get; set; }
        public List<Attachment>? attachments { get; set; }
        public List<Note>? notes { get; set; }
        public virtual List<Event>? events { get; set; }
        public virtual List<Warranty>? warranties { get; set; }

        #region Implementation of IEnumerable
        List<Quote> quotes;
        //public IEnumerator<Quote> GetEnumerator()
        //{
        //    return quotes.GetEnumerator();
        //}
        #endregion
    }

}
