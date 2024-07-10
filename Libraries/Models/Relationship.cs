using MongoDB.Bson;

namespace Facilitate.Libraries.Models
{
    public class Relationship
    {
        public string RelatedToId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
