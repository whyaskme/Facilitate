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
    public class ProfileContacts
    {
        public ProfileContacts()
        {
            UserId = string.Empty;
            Address = new List<Address>();
            Email = new List<Email>();
            Phone = new List<Phone>();
        }

        [Key]
        public string UserId { get; set; }
        public List<Address> Address { get; set; }
        public List<Email> Email { get; set; }
        public List<Phone> Phone { get; set; }
    }
}