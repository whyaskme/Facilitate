using MongoDB.Bson;

namespace Facilitate.Libraries.Models
{
    public class QuoteProductRequestedRoofleSubmission// : Base
    {
        public QuoteProductRequestedRoofleSubmission()
        {

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
        public string timestamp { get; set; }
        public int numberOfStructures { get; set; }
        public int numberOfIncludedStructures { get; set; }
        public int totalSquareFeet { get; set; }
        public int mainRoofTotalSquareFeet { get; set; }
        public int totalInitialSquareFeet { get; set; }
        public string sessionId { get; set; }

        public List<Structure>? structures { get; set; }

        public string productName { get; set; }

        public PriceInfo? priceInfo { get; set; }
        public PriceRange? priceRange { get; set; }

        public double? wasteFactorMainRoof { get; set; }
    }

}
