using System.Collections.Generic;
using System.Linq;
using TvShowTracker.Domain.Models.ShowWithProgress;

namespace TvShowTracker.WebApi.Dto
{
    public class ShowWithSeasonsDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public string Plot { get; set; }
        public int? NextSeasonNumber { get; set; }
        public int? NextEpisodeNumber { get; set; }
        public List<SeasonDto> Seasons { get; set; }
        
        
        public static ShowWithSeasonsDto FromShowWithProgress(ShowWithProgress showWithProgress)
        {
            return new ShowWithSeasonsDto
            {
                Id = showWithProgress.Show.Id,
                Title = showWithProgress.Show.Title,
                Poster = showWithProgress.Show.Poster,
                Plot = showWithProgress.Show.Plot,
                NextEpisodeNumber = showWithProgress?.NextEpisode?.EpisodeNumber,
                NextSeasonNumber = showWithProgress?.NextEpisode?.SeasonNumber,
                Seasons = showWithProgress.SeasonsWithProgress.Select(SeasonDto.FromSeasonWithProgress).ToList()
            };
        }
    }
}