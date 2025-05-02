using System.Net.Http.Json;
using BlogApp.Models;

namespace BlogApp.Services
{
    public class PostService
    {
        private readonly HttpClient _http;

        public PostService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            //try
            //{
            //    var response = await _http.GetAsync("api/posts");
            //    response.EnsureSuccessStatusCode();
            //    return await response.Content.ReadFromJsonAsync<List<Post>>();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error fetching posts: {ex.Message}");
            //    return new List<Post>();
            //}




            //var response = await _http.GetAsync("api/posts");

            //if (!response.IsSuccessStatusCode)
            //{
            //    var errorContent = await response.Content.ReadAsStringAsync();
            //    Console.WriteLine($"API Error: {errorContent}");
            //    return new List<Post>();
            //}

            //return await response.Content.ReadFromJsonAsync<List<Post>>();

            try
            {
                var response = await _http.GetAsync("api/posts");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {error}");
                    return new List<Post>();
                }

                return await response.Content.ReadFromJsonAsync<List<Post>>()
                    ?? new List<Post>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Network Error: {ex.Message}");
                return new List<Post>();
            }


        }
        //public async Task CreatePostAsync(Post post)
        //{
        //    var response = await _http.PostAsJsonAsync("api/posts", post);
        //    response.EnsureSuccessStatusCode();
        //}


        public async Task CreatePostAsync(Post post)
        {
            var response = await _http.PostAsJsonAsync("api/posts", post);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to create post: {errorContent}");
            }
        }
        //public async Task<HttpResponseMessage> CreatePostAsync(Post post)
        //{
        //    return await _http.PostAsJsonAsync("api/posts", post);
        //}

        // Add these methods to the PostService class
        public async Task<bool> ToggleLikeAsync(string postId, string userId, string userEmail)
        {
            try
            {
                var request = new
                {
                    PostId = postId,
                    UserId = userId,
                    UserEmail = userEmail
                };

                var response = await _http.PostAsJsonAsync("api/likes", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error toggling like: {errorContent}");
                    return false;
                }

                var result = await response.Content.ReadFromJsonAsync<LikeToggleResponse>();
                return result?.Liked ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception toggling like: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckLikeAsync(string postId, string userId)
        {
            var response = await _http.GetAsync($"api/likes/post/{postId}/user/{userId}");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LikeResponse>();
            return result?.IsLiked ?? false;
        }

        public async Task<int> GetLikeCountAsync(string postId)
        {
            var response = await _http.GetAsync($"api/likes/post/{postId}/count");
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<LikeCountResponse>();
            return result?.Count ?? 0;
        }
        public async Task<Post?> GetPostAsync(string postId)
        {
            return await _http.GetFromJsonAsync<Post>($"api/posts/{postId}");

        }
    }



}