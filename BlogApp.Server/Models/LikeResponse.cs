namespace BlogApp.Server.Models
{
    public class LikeResponse
    {
        public bool IsLiked { get; set; }
    }

    public class LikeToggleResponse
    {
        public bool Liked { get; set; }
    }

    public class LikeCountResponse
    {
        public int Count { get; set; }
    }
}