using Facilitate.Libraries.Models;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Facilitate.Libraries.Models.Constants.Transaction;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Facilitate.Libraries.Models
{
    public class PropertyInfo : Base
    {
        public PropertyInfo()
        {
            _t = "PropertyInfo";

            Market = string.Empty;
            Address = new Address(this._id.ToString());
            Structures = new List<Structure>();
        }

        public string Market { get; set; }
        public Address Address { get; set; }
        public List<Structure> Structures { get; set; }
    }
}
