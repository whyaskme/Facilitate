using MongoDB.Bson;

namespace Facilitate.Libraries.Models
{
    public class Relationship
    {
        public Relationship()
        {
            _id = ObjectId.GenerateNewId().ToString();
            _t = "Relationship";

            ParentId = "";
            Type = "";
            Name = "";
            IsEnabled = true;
            CreatedById = "";
            CreatedDate = DateTime.UtcNow;
        }

        public string _id { get; set; }
        public string _t { get; set; }
        public string ParentId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
