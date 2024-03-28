using System.Collections.Generic;
using System;

namespace Facilitate.Libraries.Models
{
    public class Profile
    {
        public Profile()
        {
            RegistrationDate = DateTime.UtcNow;
            Expired = false;
            ExpireDate = DateTime.MaxValue;
            DeviceType = 0;
            IsLoggedIn = false;
            Title = "Unknown";
            FirstName = "Unknown";
            MiddleName = "Unknown";
            LastName = "Unknown";
            Suffix = "Unknown";
            Gender = 0;

            Contact = new ContactInfo();
            CreditCards = new List<CreditCard>();
            Transactions = new List<Transaction>();
        }

        // Custom profile fields
        public DateTime RegistrationDate { get; set; }
        public Boolean Expired { get; set; }
        public DateTime ExpireDate { get; set; }
        public Int16 DeviceType { get; set; } // Android (Phone) = 1, Android (Tablet) = 2, IOS (Phone) - 3, IOS (Tablet) - 4
        public bool IsLoggedIn { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public int Gender { get; set; } // 0 = Not specified, Female = 1, Male = 2

        public ContactInfo Contact { get; set; }
        public List<CreditCard> CreditCards { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
