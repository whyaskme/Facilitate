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
    public class Notification : Base
    {
        public Notification(Double latitude, Double longitude)
        {
            _t = "Notification";

            UserId = ObjectId.Empty;
            Name = string.Empty;
            DateTime = DateTime.UtcNow;
            PackageName = string.Empty;
            NotificationTicker = string.Empty;
            NotificationTitle = string.Empty;
            NotificationText = string.Empty;

            Location = new Location(UserId, ObjectId.Empty, "", latitude, longitude);
        }
        public ObjectId UserId { get; set; }
        public DateTime DateTime { get; set; }
        public string PackageName { get; set; }
        public string NotificationTicker { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationText { get; set; }
        public Location Location { get; set; }
    }
}