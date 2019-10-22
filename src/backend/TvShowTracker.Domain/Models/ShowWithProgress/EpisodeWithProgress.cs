using TvShowTracker.Domain.Models.Show;

namespace TvShowTracker.Domain.Models.ShowWithProgress
{
    public class EpisodeWithProgress
    {
        public int TotalEpisodeNumber { get; set; }
        public int SeasonNumber { get; set; }
        public Episode Episode { get; set; }
        public bool IsCompleted { get; set; }
    }
}