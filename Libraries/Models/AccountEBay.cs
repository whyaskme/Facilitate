using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
//using MongoDB.Driver.Builders;

namespace Facilitate.Libraries.Models
{
    public class AccountEBay
    {
        public AccountEBay()
        {
            UserName = "";
            Password = "";
            IsRegistered = false;
            RegistrationDate = DateTime.MinValue;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsRegistered { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
