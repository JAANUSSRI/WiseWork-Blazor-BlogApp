using Microsoft.AspNetCore.Mvc;
using BlogApp.Server.Services;
using BlogApp.Server.Models;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public CommentsController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<List<Comment>>> GetCommentsForPost(string postId)
        {
            var comments = await _mongoDBService.Comments
                .Find(c => c.PostId == postId)
                .SortByDescending(c => c.CreatedAt)
                .ToListAsync();

            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var comment = new Comment
                {
                    PostId = request.PostId,
                    UserId = request.UserId,
                    UserEmail = request.UserEmail,
                    Content = request.Content,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _mongoDBService.Comments.InsertOneAsync(comment);
                return CreatedAtAction(nameof(GetCommentsForPost), new { postId = request.PostId }, comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("post/{postId}/count")]
        public async Task<ActionResult<int>> GetCommentCount(string postId)
        {
            var count = await _mongoDBService.Comments
                .CountDocumentsAsync(c => c.PostId == postId);

            return Ok((int)count);
        }
        public class CommentRequest
        {
            [Required]
            public string PostId { get; set; }

            [Required]
            public string UserId { get; set; }

            [Required]
            [EmailAddress]
            public string UserEmail { get; set; }

            [Required]
            [MinLength(1)]
            public string Content { get; set; }
        }
    }
}