using TvShowTracker.Domain.Models.ShowWithProgress;

namespace TvShowTracker.WebApi.Dto
{
    public class ShowDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Poster { get; set; }
        public int? NextSeasonNumber { get; set; }
        public int? NextEpisodeNumber { get; set; }

        public static ShowDto FromShowWithProgress(ShowWithProgress showWithProgress)
        {
            return new ShowDto
            {
                Id = showWithProgress?.Show.Id,
                Title = showWithProgress?.Show.Title,
                Poster = showWithProgress?.Show.Poster,
                NextEpisodeNumber = showWithProgress?.NextEpisode?.EpisodeNumber,
                NextSeasonNumber = showWithProgress?.NextEpisode?.SeasonNumber
            };
        }
    }
}