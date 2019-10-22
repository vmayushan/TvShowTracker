using System.Collections.Generic;

namespace TvShowTracker.DataAccess.OMDb.Models
{
    public class OMDbSeason
    {
        public int Season { get; set; }
        public List<OMDbEpisode> Episodes { get; set; }
    }
}