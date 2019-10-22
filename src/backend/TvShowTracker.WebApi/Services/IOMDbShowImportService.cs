using System.Collections.Generic;
using System.Threading.Tasks;
using TvShowTracker.DataAccess.OMDb.Models;
using TvShowTracker.Domain.Models.Show;

namespace TvShowTracker.WebApi.Services
{
    public interface IOMDbShowImportService
    {
        Task<Show> GetFromOMDb(string imdbId);
        Task<List<OMDbShowAutocomplete>> SearchByTitle(string query);
    }
}