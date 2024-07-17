using MongoDB.Bson;

namespace Facilitate.Libraries.Models
{
    public class Attachment
    {
        public Attachment()
        {
            _id = ObjectId.GenerateNewId();
            _t = "Attachment";
            Trade = string.Empty;
            IsDeleted = false;
            DateTime = DateTime.UtcNow;
            MediaDescription = string.Empty;
            MediaDescription = string.Empty;
            MediaUrl = string.Empty;
            Author = new ApplicationUser();
        }

        #region Properties

        public ObjectId _id { get; set; }
        public string _t { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateTime { get; set; }
        public string Trade { get; set; }
        public string MediaName { get; set; }
        public string MediaDescription { get; set; }
        public string MediaUrl { get; set; }
        public ApplicationUser Author { get; set; }

        #endregion
    }
}