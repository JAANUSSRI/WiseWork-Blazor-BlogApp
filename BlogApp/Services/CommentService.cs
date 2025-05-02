using System.Net.Http.Json;
using BlogApp.Models;
using BlogApp.Server.Models;

namespace BlogApp.Services
{
    public class CommentService
    {
        private readonly HttpClient _http;

        public CommentService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Comment>> GetCommentsAsync(string postId)
        {
            return await _http.GetFromJsonAsync<List<Comment>>($"api/comments/post/{postId}");
        }

        public async Task CreateCommentAsync(string postId, string userId, string userEmail, string content)
        {
            var request = new
            {
                PostId = postId,
                UserId = userId,
                UserEmail = userEmail,
                Content = content
            };

            var response = await _http.PostAsJsonAsync("api/comments", request);
            response.EnsureSuccessStatusCode();
        }
        public async Task<int> GetCommentCountAsync(string postId)
        {
            var response = await _http.GetAsync($"api/comments/post/{postId}/count");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<int>();
            }
            return 0;
        }
    }
}