using System.Collections.Generic;

namespace TvShowTracker.Domain.Models.Show
{
    public class Show
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public List<Season> Seasons { get; set; }
    }
}