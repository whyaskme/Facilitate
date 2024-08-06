using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class Note
    {
        public Note()
        {
            _id = ObjectId.GenerateNewId();
            _t = "Note";
            Trade = string.Empty;
            IsDeleted = false;
            Relationship = string.Empty;
            DateTime = DateTime.UtcNow;
            Summary = string.Empty;
            Details = string.Empty;
            Author = new ApplicationUser();
        }

        #region Properties

        public ObjectId _id { get; set; }
        public string _t { get; set; }
        public string Trade { get; set; }
        public bool IsDeleted { get; set; }
        public string Relationship { get; set; }
        public DateTime DateTime { get; set; }
        public string Summary { get; set; }
        public string Details { get; set; }
        public ApplicationUser Author { get; set; }

        #endregion
    }
}
