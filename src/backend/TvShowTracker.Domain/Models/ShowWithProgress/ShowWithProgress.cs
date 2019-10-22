using System.Collections.Generic;

namespace TvShowTracker.Domain.Models.ShowWithProgress
{
    public class ShowWithProgress
    {
        public Show.Show Show { get; set; }
        public bool IsCompleted { get; set; }
        public NextShowEpisode NextEpisode { get; set; }
        public List<SeasonWithProgress> SeasonsWithProgress { get; set; }
    }
}