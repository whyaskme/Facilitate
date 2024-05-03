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

            RegistrationDate = DateTime.UtcNow;
            Expired = false;
            ExpireDate = DateTime.MaxValue;

            Title = string.Empty;
            FirstName = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            Suffix = string.Empty;
            Gender = 0;

            Address1 = string.Empty;
            Address2 = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;

            Email = string.Empty;
            Phone = string.Empty;
        }

        public UserProfile(string _id)
        {
            Id = _id;

            RegistrationDate = DateTime.UtcNow;
            Expired = false;
            ExpireDate = DateTime.MaxValue;

            Title = string.Empty;
            FirstName = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            Suffix = string.Empty;
            Gender = 0;

            Address1 = string.Empty;
            Address2 = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;

            Email = string.Empty;
            Phone = string.Empty;
        }

        [Key]
        public string Id { get; set; }

        public DateTime RegistrationDate { get; set; }
        public Boolean Expired { get; set; }
        public DateTime ExpireDate { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public int Gender { get; set; } // 0 = Not specified, Female = 1, Male = 2

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
