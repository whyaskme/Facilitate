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

            RepLead = new Name();
            RepEmail = new Email();
            RepPhone = new Phone();
            RepCompany = string.Empty;
        }

        public Name RepLead { get; set; }
        public Email RepEmail { get; set; }
        public Phone RepPhone{ get; set; }
        public string RepCompany { get; set; }
    }
}
