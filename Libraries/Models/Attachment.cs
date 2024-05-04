using MongoDB.Bson;

namespace Facilitate.Libraries.Models
{
    public class Attachment
    {
        public Attachment()
        {
            _id = ObjectId.GenerateNewId();
            _t = "Attachment";
            IsDeleted = false;
            Date = DateTime.UtcNow;
            MediaDescription = string.Empty;
            MediaUrl = string.Empty;
            Author = new ApplicationUser();
        }

        #region Properties

        public ObjectId _id { get; set; }
        public string _t { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Date { get; set; }
        public string MediaDescription { get; set; }
        public string MediaUrl { get; set; }
        public ApplicationUser Author { get; set; }

        #endregion
    }
}