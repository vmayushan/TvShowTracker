using System.Collections.Generic;

namespace TvShowTracker.Domain.Models.Show
{
    public class Season
    {
        public int SeasonNumber { get; set; }
        public List<Episode> Episodes { get; set; }
    }
}