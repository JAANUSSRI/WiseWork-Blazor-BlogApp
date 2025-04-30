using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;

namespace BlogApp.Services
{
    public class AuthService
    {
        private readonly Supabase.Client _supabase;
        private readonly ProfileService _profileService;
        public event Action OnAuthStateChanged;

        public AuthService(Supabase.Client supabase, ProfileService profileService)
        {
            _supabase = supabase;
            _profileService = profileService;
        }

        public async Task<Session?> SignUp(string email, string password)
        {
            try
            {
                var session = await _supabase.Auth.SignUp(email, password);
                OnAuthStateChanged?.Invoke();
                if (session?.User != null)
                {
                    await _profileService.CreateProfileAsync(session.User); // This creates the profile
                }
                return session;
            }
            catch (GotrueException ex)
            {
                throw new Exception("Sign up failed. Please try again.");
            }
        }

        public async Task<Session?> SignIn(string email, string password)
        {
            try
            {
                var session = await _supabase.Auth.SignIn(email, password);
                OnAuthStateChanged?.Invoke();
                return session;
            }
            catch (GotrueException ex)
            {
                throw new Exception("Invalid email or password.");
            }
        }

        public async Task SignOut()
        {
            await _supabase.Auth.SignOut();
            OnAuthStateChanged?.Invoke();
        }

        public async Task<User?> GetUser()
        {
            return _supabase.Auth.CurrentUser;
        }
    }
}

