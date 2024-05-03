using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class Email
    {
        public Email()
        {
            UserName = string.Empty;
            Domain = string.Empty;
        }

        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
    }
}