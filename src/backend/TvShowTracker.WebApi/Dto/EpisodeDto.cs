using TvShowTracker.Domain.Models.ShowWithProgress;

namespace TvShowTracker.WebApi.Dto
{
    public class EpisodeDto
    {
        public bool IsCompleted { get; set; }
        public int EpisodeNumber { get; set; }
        public string Title { get; set; }
        public string ImdbId { get; set; }

        public static EpisodeDto FromEpisodeWithProgress(EpisodeWithProgress episode)
        {
            return new EpisodeDto
            {
                IsCompleted = episode.IsCompleted,
                ImdbId = episode.Episode.ImdbId,
                Title = episode.Episode.Title,
                EpisodeNumber = episode.Episode.EpisodeNumber
            };
        }
    }
}