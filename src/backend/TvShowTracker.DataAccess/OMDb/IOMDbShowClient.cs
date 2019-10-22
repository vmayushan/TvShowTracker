using System.Collections.Generic;
using System.Threading.Tasks;
using TvShowTracker.DataAccess.OMDb.Models;

namespace TvShowTracker.DataAccess.OMDb
{
    public interface IOMDbShowClient
    {
        Task<List<OMDbShowAutocomplete>> SearchShowsByTitle(string query);
        Task<OMDbShow> GetShowByImdbId(string imdbId);
        Task<OMDbSeason> GetSeason(string imdbId, int seasonNumber);
    }
}