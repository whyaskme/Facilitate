using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json;

namespace Facilitate.Libraries.Models
{
    public class AttachmentImage
    {
        public AttachmentImage()
        {
            _id = ObjectId.GenerateNewId();
            _t = "ImageAttachment";
            Date = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            Media = ""; // ConfigurationManager.AppSettings["ServiceHost"] + ConfigurationManager.AppSettings["TimePopImageUrl"];
        }

        #region Properties

        public ObjectId _id { get; set; }
        public string _t { get; set; }
        public string Date { get; set; }
        public string Media { get; set; }

        #endregion
    }
}