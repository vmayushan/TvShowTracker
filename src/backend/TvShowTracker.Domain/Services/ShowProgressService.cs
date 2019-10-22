using System.Linq;
using TvShowTracker.Domain.Models.Show;
using TvShowTracker.Domain.Models.ShowProgress;
using TvShowTracker.Domain.Models.ShowWithProgress;

namespace TvShowTracker.Domain.Services
{
    public class ShowProgressService : IShowProgressService
    {
        public ShowWithProgress MergeShowWithProgress(Show show, ShowProgress progress)
        {
            var showWithProgress = new ShowWithProgress { Show = show };

            var seasons = show.Seasons.OrderBy(x => x.SeasonNumber).Select(
                season => MergeSeasonWithProgress(progress, season)
            ).ToList();

            showWithProgress.IsCompleted = seasons.All(x => x.IsCompleted);
            showWithProgress.SeasonsWithProgress = seasons;
            
            SetTotalEpisodeNumbers(showWithProgress);
            
            showWithProgress.NextEpisode = GetNextShowEpisode(showWithProgress);
            
            return showWithProgress;
        }

        private static void SetTotalEpisodeNumbers(ShowWithProgress showWithProgress)
        {
            var episodeNumber = 0;
            foreach (var episode in showWithProgress
                .SeasonsWithProgress
                .SelectMany(season => season.EpisodesWithProgress))
            {
                episodeNumber++;
                episode.TotalEpisodeNumber = episodeNumber;
            }
        }

        private static NextShowEpisode GetNextShowEpisode(ShowWithProgress showWithProgress)
        {
            var allEpisodes = showWithProgress.SeasonsWithProgress
                .SelectMany(x => x.EpisodesWithProgress)
                .ToList();
            
            var lastWatchedEpisodeNumber = allEpisodes
                .Where(x => x.IsCompleted)
                .OrderByDescending(x => x.TotalEpisodeNumber)
                .Select(x => x.TotalEpisodeNumber)
                .FirstOrDefault();

            var nextEpisode = allEpisodes
                .Where(x => !x.IsCompleted && x.TotalEpisodeNumber > lastWatchedEpisodeNumber)
                .OrderBy(x => x.TotalEpisodeNumber)
                .FirstOrDefault();

            if (nextEpisode == null) return null;

            return new NextShowEpisode
            {
                EpisodeNumber = nextEpisode.Episode.EpisodeNumber,
                SeasonNumber = nextEpisode.SeasonNumber
            };
        }

        private static SeasonWithProgress MergeSeasonWithProgress(
            ShowProgress progress, Season season)
        {
            var seasonWithProgress = new SeasonWithProgress { Season = season };

            var episodes = season.Episodes.OrderBy(x => x.EpisodeNumber).Select(
                episode => MergeEpisodeWithProgress(
                    progress, season.SeasonNumber, episode
                )
            ).ToList();

            seasonWithProgress.IsCompleted = episodes.All(x => x.IsCompleted);
            seasonWithProgress.EpisodesWithProgress = episodes;

            return seasonWithProgress;
        }

        private static EpisodeWithProgress MergeEpisodeWithProgress(
            ShowProgress progress, int seasonNumber, Episode episode)
        {
            return new EpisodeWithProgress
            {
                Episode = episode,
                SeasonNumber = seasonNumber,
                IsCompleted = IsWatchedEpisode(progress, seasonNumber, episode.EpisodeNumber )
            };
        }

        private static bool IsWatchedEpisode(
            ShowProgress progress, int season, int episode)
        {
            return progress?.WatchedEpisodes?.Any(
                       x => x.EpisodeNumber == episode && x.SeasonNumber == season
                   ) ?? false;
        }
    }
}