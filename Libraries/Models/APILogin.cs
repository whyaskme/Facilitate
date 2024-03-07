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
    public class ApiLogin
    {
        public ApiLogin()
        {
            Date = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            ApiUrl = "";
            AppId = "";
            AppSecretKey = "";
            UserName = "";
            Password = "";
        }
        public string Date { get; set; }
        public string ApiUrl { get; set; }
        public string AppId { get; set; }
        public string AppSecretKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
    }
}