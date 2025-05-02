using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver; 
using BlogApp.Server.Services;
using BlogApp.Server.Models;
using MongoDB.Bson;

namespace BlogApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public PostsController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> Get()
        {
            //return await _mongoDBService.Posts.Find(_ => true).ToListAsync();
            ////try
            ////{
            ////    var posts = await _mongoDBService.Posts.Find(_ => true).ToListAsync();
            ////    return Ok(posts); // Explicitly return 200 OK with data
            ////}
            ////catch (Exception ex)
            ////{
            ////    return StatusCode(500, $"Internal server error: {ex.Message}");
            ////}

            try
            {
                var posts = await _mongoDBService.Posts.Find(_ => true).ToListAsync();
                return Ok(posts); // Explicit return
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            //post.Id = ObjectId.GenerateNewId().ToString();
            //await _mongoDBService.Posts.InsertOneAsync(post);
            //return CreatedAtAction(nameof(Get), post);

            ModelState.Remove("Id");

            if (string.IsNullOrEmpty(post.Title))
                ModelState.AddModelError("Title", "Title is required");

            if (string.IsNullOrEmpty(post.Content))
                ModelState.AddModelError("Content", "Content is required");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //post.Id = ObjectId.GenerateNewId().ToString();
            //await _mongoDBService.Posts.InsertOneAsync(post);
            //return CreatedAtAction(nameof(Get), new { id = post.Id }, post);

            try
            {
                post.Id = ObjectId.GenerateNewId().ToString();
                post.CreatedAt = DateTime.UtcNow;
                post.UpdatedAt = DateTime.UtcNow;

                await _mongoDBService.Posts.InsertOneAsync(post);
                return CreatedAtAction(nameof(Get), post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


            //try
            //{
            //    // Set timestamps
            //    post.CreatedAt = DateTime.UtcNow;
            //    post.UpdatedAt = DateTime.UtcNow;

            //    // Generate new ID if not provided
            //    if (string.IsNullOrEmpty(post.Id))
            //    {
            //        post.Id = ObjectId.GenerateNewId().ToString();
            //    }

            //    await _mongoDBService.Posts.InsertOneAsync(post);
            //    return CreatedAtAction(nameof(Get), post);
            //}
            //catch (Exception ex)
            //{
            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            //}


        }

        // Add to BlogApp.Server/Controllers/PostsController.cs
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(string id)
        {
            var post = await _mongoDBService.Posts.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }
    }
}