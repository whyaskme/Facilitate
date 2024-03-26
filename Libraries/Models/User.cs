﻿using System;
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

        //public List<Role> Roles { get; set; }
        public string[] Roles { get; set; }

        public List<Claim> Claims { get; set; }

        public List<Login> Logins { get; set; }
        //public string[] Login { get; set; }

        public List<Token> Tokens { get; set; }
    }
}