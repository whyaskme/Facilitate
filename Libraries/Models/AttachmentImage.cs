using MongoDB.Bson;

namespace Facilitate.Libraries.Models
{
    public class AttachmentImage
    {
        public AttachmentImage()
        {
            _id = ObjectId.GenerateNewId();
            _t = "ImageAttachment";
            Date = DateTime.UtcNow.ToString();
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