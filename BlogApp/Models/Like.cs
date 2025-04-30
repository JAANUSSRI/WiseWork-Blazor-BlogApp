using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogApp.Server.Models
{
    public class Like
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string PostId { get; set; }
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}