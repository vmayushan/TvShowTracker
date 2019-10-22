namespace TvShowTracker.WebApi.Configuration
{
    public class MongoSettings
    {
        public const string ConfigSectionKey = "Mongo";
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}