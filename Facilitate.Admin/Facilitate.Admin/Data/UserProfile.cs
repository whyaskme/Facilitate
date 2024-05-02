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
            UserName = string.Empty;
            Domain = string.Empty;
        }

        [Key]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Domain { get; set; }
    }
}
