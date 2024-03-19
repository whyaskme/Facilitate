using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class Location
    {
        public Location(ObjectId referenceId, String deviceType, String userIP, Double latitude, Double longitude)
        {
            DateTime = DateTime.UtcNow;
            DeviceType = deviceType;
            UserId = referenceId;
            UserIP = userIP;
            Longitude = longitude;
            Latitude = latitude;
        }

        public DateTime DateTime { get; set; }
        public String DeviceType { get; set; }
        public ObjectId UserId { get; set; }
        public String UserIP { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
}