namespace TvShowTracker.DataAccess.OMDb.Models
{
    public class OMDbEpisode
    {
        public string Title { get; set; }
        public string Released { get; set; }
        public int Episode { get; set; }
        public string ImdbId { get; set; }
        public string ImdbRating { get; set; }
    }
}