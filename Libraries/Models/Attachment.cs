using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Facilitate.Libraries.Models
{
    public class Attachment
    {
        public Attachment()
        {
            _id = ObjectId.GenerateNewId();
            _t = "Attachment";
            Date = DateTime.UtcNow;
            MediaDescription = "";
            MediaUrl = "";
        }

        #region Properties

        public ObjectId _id { get; set; }
        public string _t { get; set; }
        public DateTime Date { get; set; }
        public string MediaDescription { get; set; }
        public string MediaUrl { get; set; }

        #endregion
    }
}