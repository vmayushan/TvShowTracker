namespace TvShowTracker.WebApi.Configuration
{
    public class OMDbApiSettings
    {
        public const string ConfigSectionKey = "OMDbApi";
        public string ApiKey { get; set; }
        public string BaseAddress { get; set; }
    }
}