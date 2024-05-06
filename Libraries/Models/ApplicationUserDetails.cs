using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilitate.Libraries.Models
{
    public class ApplicationUserDetails : ApplicationUser
    {
        public ApplicationUserDetails()
        {
            Roles = new List<ListItem>();
        }

        public List<ListItem> Roles { get; set; }
    }
}
