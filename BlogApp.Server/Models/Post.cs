using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlogApp.Server.Models
{
    //public class Post
    //{
    //    [BsonId]
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string Id { get; set; }

    //    public string Title { get; set; }
    //    public string Content { get; set; }
    //    public string Summary { get; set; }
    //    public string AuthorId { get; set; }
    //    public string AuthorEmail { get; set; }
    //    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    //}
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Make nullable

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string Summary { get; set; }
        public string AuthorId { get; set; }
        public string AuthorEmail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}