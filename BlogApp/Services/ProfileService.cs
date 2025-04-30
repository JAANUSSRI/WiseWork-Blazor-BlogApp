

using System.Net.Http.Json;
using BlogApp.Models;
using Supabase.Gotrue;

namespace BlogApp.Services
{
    public class ProfileService
    {
        private readonly HttpClient _http;

        public ProfileService(HttpClient http)
        {
            _http = http;
        }
        public async Task CreateProfileAsync(User user)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("api/profiles", user);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Profile creation failed: {ex.Message}");
                throw;
            }
        }
    }
}