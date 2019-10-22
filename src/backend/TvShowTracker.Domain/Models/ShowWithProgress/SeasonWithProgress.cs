using System.Collections.Generic;
using TvShowTracker.Domain.Models.Show;

namespace TvShowTracker.Domain.Models.ShowWithProgress
{
    public class SeasonWithProgress
    {
        public Season Season { get; set; }
        public bool IsCompleted { get; set; }
        public List<EpisodeWithProgress> EpisodesWithProgress { get; set; }
    }
}