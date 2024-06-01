using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class QuoteAddressOnlyRoofleSubmission
    {
        public string? address { get; set; }
        public string? externalUrl { get; set; }
        public string? sessionId { get; set; }
    }
}
