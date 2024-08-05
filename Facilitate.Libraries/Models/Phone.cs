using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class Phone
    {
        public Phone()
        {
            Id = string.Empty;
            PhoneType = 0; // 0=Mobile, 1=Home, 2=Work, 3=Fax
            CountryCode = 1; // 1 = United States
            AreaCode = 000;
            Exchange = 000;
            Number = 0000;
        }

        public Phone(string _id)
        {
            Id = _id;
            PhoneType = 0; // 0=Mobile, 1=Home, 2=Work, 3=Fax
            CountryCode = 1; // 1 = United States
            AreaCode = 000;
            Exchange = 000;
            Number = 0000;
        }

        [Key]
        public string Id { get; set; }
        public int PhoneType { get; set; }
        public int CountryCode { get; set; }
        public int AreaCode { get; set; }
        public int Exchange { get; set; }
        public int Number { get; set; }
    }
}