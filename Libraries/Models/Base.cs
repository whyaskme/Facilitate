using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;

using Facilitate.Libraries.Models;
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
    /// <summary>
    /// 
    /// </summary>
    public class Base
    {

        /// <summary>
        /// 
        /// </summary>
        public Base()
        {
            _id = ObjectId.GenerateNewId();
            _t = "Base";

            Enabled = true;

            Name = string.Empty;
            Date = DateTime.Now.Date;
        }
        /// <summary>
        /// 
        /// </summary>
        public ObjectId _id { get; set; }
        //public string _id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string _t { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Enabled { get; set; }
        public string Name { get; set; }

        public DateTime? Date { get; set; }
    }
}