using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Server.Models
{
    public class Like
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public string PostId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string UserEmail { get; set; }

        public DateTime LikedAt { get; set; } = DateTime.UtcNow;
    }
}