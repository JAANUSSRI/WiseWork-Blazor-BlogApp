using Microsoft.AspNetCore.Mvc;
using BlogApp.Server.Services;
using BlogApp.Server.Models;
using Supabase.Gotrue;

namespace BlogApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public ProfilesController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            var profile = new UserProfile
            {
                UserId = user.Id,
                Email = user.Email ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await _mongoDBService.Profiles.InsertOneAsync(profile);
            return Ok();
        }
    }
}