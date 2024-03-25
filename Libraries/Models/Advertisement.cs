using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
//using MongoDB.Driver.Builders;

namespace Facilitate.Libraries.Models
{
    public class Advertisement : Base
    {
        public Advertisement()
        {
            _t = "Advertisement";

            DateModified = DateTime.UtcNow;

            IsDeleted = false;

            EmailAdress = "";
            EmailAdress = "";

            ChkContactPhone = false;
            ChkContactText = false;

            ContactPhoneNumber = "";
            ContactPhoneExt = "";
            ContactName = "";

            AdStatus = "Created";
            AdPrice = "";
            AdTitle = "";
            AdDescription = "";

            AdImages = new List<Attachment>();
            ItemStates = new List<State>();
            //ItemCities = new List<City>();
        }

        public DateTime DateModified { get; set; }

        public bool IsDeleted { get; set; }

        public string EmailAdress { get; set; }
        public string EmailRelay { get; set; }

        public bool ChkContactPhone { get; set; }
        public bool ChkContactText { get; set; }

        public string ContactPhoneNumber { get; set; }
        public string ContactPhoneExt { get; set; }
        public string ContactName { get; set; }

        public string AdStatus { get; set; }
        public string AdPrice { get; set; }
        public string AdTitle { get; set; }
        public string AdDescription { get; set; }

        public List<Attachment> AdImages { get; set; }
        public List<State> ItemStates { get; set; }

        //public string Save()
        //{
        //    var myUtils = new Utils();

        //    string serviceResponse = myUtils.SaveAdvertisement(this);

        //    return serviceResponse = "Successful";;
        //}
    }
}
