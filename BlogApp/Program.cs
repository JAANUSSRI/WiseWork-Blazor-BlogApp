using BlogApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Supabase;
using BlogApp.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    //BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    BaseAddress = new Uri("https://localhost:7030") // Update this to match your backend URL

});

// Register Supabase
builder.Services.AddScoped(provider =>
    new Client(
        "https://afkaopyppyivobkpaoow.supabase.co",
        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImFma2FvcHlwcHlpdm9ia3Bhb293Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDQ3NTYzMjMsImV4cCI6MjA2MDMzMjMyM30.NWixuU0BslF0uCYGxT0eefhDDSXBoxEAIo03F4jjnDI",
        new SupabaseOptions
        {
            AutoRefreshToken = true,
            AutoConnectRealtime = true
        }
    )
);

// Register services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<CommentService>();

await builder.Build().RunAsync();
