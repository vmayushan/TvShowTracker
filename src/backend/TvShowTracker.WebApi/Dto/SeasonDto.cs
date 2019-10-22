using System.Collections.Generic;
using System.Linq;
using TvShowTracker.Domain.Models.ShowWithProgress;

namespace TvShowTracker.WebApi.Dto
{
    public class SeasonDto
    {
        public int SeasonNumber { get; set; }
        public bool IsCompleted { get; set; }
        public List<EpisodeDto> Episodes { get; set; }

        public static SeasonDto FromSeasonWithProgress(SeasonWithProgress season)
        {
            return new SeasonDto
            {
                SeasonNumber = season.Season.SeasonNumber,
                IsCompleted = season.IsCompleted,
                Episodes = season.EpisodesWithProgress.Select(EpisodeDto.FromEpisodeWithProgress).ToList()
            };
        }
    }
}