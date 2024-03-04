using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class ReferenceData
    {
        public ReferenceData()
        {
            PhoneType = 0; // 0=Mobile, 1=Home, 2=Work, 3=Fax
        }

        public int PhoneType { get; set; }
    }
}
