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
            UserProfile = new UserProfile(this.Id);
        }

        public UserProfile UserProfile { get; set; }
    }
}
