using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Facilitate.Libraries.Models
{
    public class User // : Base
    {
        public User()
        {
            //_id = ObjectId.GenerateNewId();
        }

        public ObjectId _id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public Int32 AccessFailedCount { get; set; }

        public string[] Roles { get; set; }

        public List<Claim> Claims { get; set; }
        public List<Login> Logins { get; set; }
        public List<Token> Tokens { get; set; }

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
        public string Pwd { get; set; }
        public int Gender { get; set; } // 0 = Not specified, Female = 1, Male = 2

        public ContactInfo Contact { get; set; }
        public List<CreditCard> CreditCards { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}