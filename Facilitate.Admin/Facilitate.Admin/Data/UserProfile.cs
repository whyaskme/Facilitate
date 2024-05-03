using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


using System.ComponentModel.DataAnnotations;

using Facilitate.Libraries.Models;

namespace Facilitate.Admin.Data
{
    public class UserProfile
    {
        public UserProfile()
        {
            Id = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            ContactInfo = new ContactInfo();
        }

        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ContactInfo ContactInfo { get; set; }
    }
}
