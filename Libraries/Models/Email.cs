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
            Id = string.Empty;
            UserName = string.Empty;
            Domain = string.Empty;
        }

        public Email(string _id)
        {
            Id = _id;
            UserName = string.Empty;
            Domain = string.Empty;
        }

        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
    }
}