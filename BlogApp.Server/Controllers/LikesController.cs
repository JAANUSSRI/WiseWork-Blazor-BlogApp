using Microsoft.AspNetCore.Mvc;
using BlogApp.Server.Services;
using BlogApp.Server.Models;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public LikesController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpPost]

        public async Task<ActionResult<LikeToggleResponse>> ToggleLike([FromBody] LikeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingLike = await _mongoDBService.Likes
                    .Find(l => l.PostId == request.PostId && l.UserId == request.UserId)
                    .FirstOrDefaultAsync();

                if (existingLike != null)
                {
                    await _mongoDBService.Likes.DeleteOneAsync(l => l.Id == existingLike.Id);
                    return Ok(new LikeToggleResponse { Liked = false });
                }
                else
                {
                    var like = new Like
                    {
                        PostId = request.PostId,
                        UserId = request.UserId,
                        UserEmail = request.UserEmail,
                        LikedAt = DateTime.UtcNow
                    };

                    await _mongoDBService.Likes.InsertOneAsync(like);
                    return Ok(new LikeToggleResponse { Liked = true });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        public class LikeRequest
        {
            [Required]
            public string PostId { get; set; }

            [Required]
            public string UserId { get; set; }

            [Required]
            [EmailAddress]
            public string UserEmail { get; set; }
        }

        [HttpGet("post/{postId}/user/{userId}")]
        public async Task<ActionResult<LikeResponse>> CheckLike(string postId, string userId)
        {
            var like = await _mongoDBService.Likes
                .Find(l => l.PostId == postId && l.UserId == userId)
                .FirstOrDefaultAsync();

            return Ok(new LikeResponse { IsLiked = like != null });
        }

        [HttpGet("post/{postId}/count")]
        public async Task<ActionResult<LikeCountResponse>> GetLikeCount(string postId)
        {
            var count = await _mongoDBService.Likes
                .CountDocumentsAsync(l => l.PostId == postId);

            return Ok(new LikeCountResponse { Count = (int)count });
        }
    }
}