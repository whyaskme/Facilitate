using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class Message
    {
        public Message()
        {
            _t = "Message";
            _id = ObjectId.GenerateNewId();
            isHtml = true;
            Date = DateTime.UtcNow;
            UserId = ObjectId.Empty;
            FromEmail = string.Empty;
            FromName = string.Empty;
            Subject = string.Empty;
            Body = string.Empty;
            ToEmail = string.Empty;
            ToName = string.Empty;
            Location = new Location(ObjectId.Empty, "", "", 0.00, 0.00);

            Status = Constants.Messaging.Status.None;
        }

        public String _t { get; set; }
        public ObjectId _id { get; set; }
        public Boolean isHtml { get; set; }
        public DateTime Date { get; set; }
        public ObjectId UserId { get; set; }
        public String FromEmail { get; set; }
        public String FromName { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
        public String ToEmail { get; set; }
        public String ToName { get; set; }
        public Location Location { get; set; }
        public Int16 Status { get; set; }
    }
}