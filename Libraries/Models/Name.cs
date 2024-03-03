using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facilitate.Libraries.Models
{
    public class Name
    {
        public Name()
        {
            Title = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Suffix = string.Empty;
        }

        public string Title { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Suffix { get; set; }
    }
}