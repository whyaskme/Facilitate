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
    public class Representative : Base
    {
        public Representative()
        {
            _t = "Representative";

            repName = new Name();
            RepEmail = new Email(this._id.ToString());
            RepPhone = new Phone(this._id.ToString());
            RepCompany = string.Empty;
        }

        public Name repName { get; set; }
        public Email RepEmail { get; set; }
        public Phone RepPhone{ get; set; }
        public string RepCompany { get; set; }
    }
}
