using MongoDB.Bson;

namespace Facilitate.Desktop.Models
{
    public class ProjectManagerSummary
    {
        public ProjectManagerSummary() 
        {
        
        }

        public ObjectId _id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
    }
}
