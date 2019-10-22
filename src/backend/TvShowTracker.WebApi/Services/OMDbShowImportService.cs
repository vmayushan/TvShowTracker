using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TvShowTracker.DataAccess.OMDb;
using TvShowTracker.DataAccess.OMDb.Models;
using TvShowTracker.Domain.Models.Show;

namespace TvShowTracker.WebApi.Services
{
    public class OMDbShowImportService : IOMDbShowImportService
    {
        private readonly IOMDbShowClient _omdbShowClient;
        private readonly ILogger<IOMDbShowImportService> _logger;

        public OMDbShowImportService(IOMDbShowClient omdbShowClient, 
            ILogger<IOMDbShowImportService> logger)
        {
            _omdbShowClient = omdbShowClient;
            _logger = logger;
        }
        
        public Task<List<OMDbShowAutocomplete>> SearchByTitle(string query)
        {
            return _omdbShowClient.SearchShowsByTitle(query);
        }

        public async Task<Show> GetFromOMDb(string imdbId)
        {
            var totalSeasons = 0;
            var omdbShow = await _omdbShowClient.GetShowByImdbId(imdbId);
            int.TryParse(omdbShow.TotalSeasons, out totalSeasons);
            var show = new Show
            {
                Id = omdbShow.ImdbId,
                Plot = omdbShow.Plot,
                Poster = omdbShow.Poster,
                Title = omdbShow.Title,
                Seasons = new List<Season>(totalSeasons)
            };
            
            for (var seasonNumber = 1; seasonNumber <= totalSeasons; seasonNumber++)
            {
                try
                {
                    var season = await ImportSeasonFromOMDb(imdbId, seasonNumber);
                    show.Seasons.Add(season);
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed to fetch season data, id = {imdbId}, season = {season}", 
                        e, imdbId, seasonNumber);
                }
            }

            return show;
        }

        private async Task<Season> ImportSeasonFromOMDb(string imdbId, int seasonNumber)
        {
            var omdbSeason = await _omdbShowClient.GetSeason(imdbId, seasonNumber);
            var season = new Season
            {
                SeasonNumber = seasonNumber,
                Episodes = new List<Episode>(omdbSeason.Episodes.Count)
            };

            for (var i = 0; i < omdbSeason.Episodes.Count; i++)
            {
                var episodeNumber = i + 1;
                var omdbEpisode = omdbSeason.Episodes[i];
                season.Episodes.Add(new Episode
                {
                    EpisodeNumber = episodeNumber,
                    Title = omdbEpisode.Title,
                    ImdbId = omdbEpisode.ImdbId
                });
            }

            return season;
        }
    }
}