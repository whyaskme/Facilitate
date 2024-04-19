using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Desktop.Models
{
    public class Note
    {
        public Note()
        {
            _id = ObjectId.GenerateNewId();
            _t = "Note";
            IsDeleted = false;
            Date = DateTime.UtcNow;
            Summary = "";
            Details = "";
        }

        #region Properties

        public ObjectId _id { get; set; }
        public string _t { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Date { get; set; }
        public string Summary { get; set; }
        public string Details { get; set; }

        #endregion
    }
}
