using MongoDB.Driver;
using BlogApp.Server.Models;

namespace BlogApp.Server.Services
{
    public class MongoDBService
    {
        private readonly IMongoDatabase _database;

        public MongoDBService(IConfiguration configuration)
        {

            //var connectionString = configuration["MongoDB:ConnectionString"]
            //    ?? throw new ArgumentNullException("MongoDB:ConnectionString is missing in configuration");

            //var databaseName = configuration["MongoDB:DatabaseName"]
            //    ?? throw new ArgumentNullException("MongoDB:DatabaseName is missing in configuration");

            ////var client = new MongoClient(configuration["MongoDB:ConnectionString"]);
            //var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            //settings.ConnectTimeout = TimeSpan.FromSeconds(60); // Increase to 60 seconds
            //settings.ServerSelectionTimeout = TimeSpan.FromSeconds(60); // Increase server selection timeout

            //var client = new MongoClient(connectionString);
            //_database = client.GetDatabase(databaseName);



            var connectionString = configuration["MongoDB:ConnectionString"]
                ?? throw new ArgumentNullException("MongoDB:ConnectionString");
            var databaseName = configuration["MongoDB:DatabaseName"]
                ?? "BlogDB"; // Default value

            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(30);

            var client = new MongoClient(settings);
            _database = client.GetDatabase(databaseName);


        }

        public IMongoCollection<Post> Posts => _database.GetCollection<Post>("Posts");
        public IMongoCollection<UserProfile> Profiles => _database.GetCollection<UserProfile>("Profiles");
        public IMongoCollection<Like> Likes => _database.GetCollection<Like>("Likes");
    }

    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
    }
}



//using MongoDB.Driver;
//using BlazorApp.Server.Models;

//namespace BlazorApp.Server.Services
//{

//    public class MongoDBService
//    {
//        private readonly IMongoDatabase _database;

//        public MongoDBService(IConfiguration configuration)
//        {
//            //var connectionString = configuration.GetConnectionString("MongoDB:ConnectionString");
//            var connectionString = configuration.GetConnectionString("MongoDB:ConnectionString");
//            // Increase the connection timeout
//            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
//            settings.ConnectTimeout = TimeSpan.FromSeconds(60); // Increase to 60 seconds
//            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(60); // Increase server selection timeout

//            //var client = new MongoClient(connectionString);
//            var client = new MongoClient(settings);
//            _database = client.GetDatabase("WeatherForecast");
//        }

//        public IMongoCollection<T> GetCollection<T>(string collectionName)
//        {
//            return _database.GetCollection<T>(collectionName);
//        }
//    }
//    public class MongoDBSettings
//    {
//        public string ConnectionString { get; set; } = string.Empty;
//    }
//}