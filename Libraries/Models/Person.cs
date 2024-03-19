using System;
using System.Collections;
using System.Collections.Generic;
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
    public class Person : Base
    {
        public Person()
        {
            _t = "Person";

            FirstName = "";
            MiddleInitial = "";
            LastName = "";
            Gender = "";
            DoB = "";

            Address = new Address();
            Contact = new ContactInfo();

            Market myMarket = new Market();

            Markets = new List<Market>();
            Markets.Add(myMarket);
        }

        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DoB { get; set; }

        public Address Address { get; set; }
        public ContactInfo Contact { get; set; }

        public List<Market> Markets { get; set; }
    }
}
