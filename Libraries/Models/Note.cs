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
            Date = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            Summary = "";
            Details = "";
        }

        #region Properties

        public ObjectId _id { get; set; }
        public string _t { get; set; }
        public string Date { get; set; }
        public string Summary { get; set; }
        public string Details { get; set; }

        #endregion
    }
}
