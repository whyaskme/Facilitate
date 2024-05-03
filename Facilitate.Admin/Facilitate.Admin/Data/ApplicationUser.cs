using Microsoft.AspNetCore.Identity;

using Facilitate.Libraries.Models;
using System.ComponentModel.DataAnnotations;

namespace Facilitate.Admin.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            IsEnabled = true;
            RegistrationDate = DateTime.UtcNow;
            Expired = false;
            ExpireDate = DateTime.MaxValue;

            Title = string.Empty;
            FirstName = string.Empty;
            MiddleName = string.Empty;
            LastName = string.Empty;
            Suffix = string.Empty;
            Gender = 0; // 0 = Not specified, Female = 1, Male = 2

            Address1 = string.Empty;
            Address2 = string.Empty;
            City = string.Empty;
            State = string.Empty;
            Zip = string.Empty;

            Email = string.Empty;
            Phone = string.Empty;

            ProfileImage = string.Empty;
        }

        public bool IsEnabled { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Boolean Expired { get; set; }
        public DateTime ExpireDate { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public int Gender { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string Phone { get; set; }
        public string ProfileImage { get; set; }
    }
}
