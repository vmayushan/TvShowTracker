using System.Collections.Generic;
using System.Threading.Tasks;
using TvShowTracker.Domain.Models.ShowProgress;

namespace TvShowTracker.DataAccess.Storage.Dao
{
    public interface IShowProgressDao
    {
        Task<ShowProgress> InsertIfNotExists(string showId, string userLogin);
        Task DeleteByShowId(string id, string userLogin);
        Task<ShowProgress> GetByShowId(string id, string userLogin);
        Task<List<ShowProgress>> GetAllByUserLogin(string userLogin);
        Task<ShowProgress> AddWatchedEpisodes(string id, string userLogin, IEnumerable<EpisodeProgress> episodes);
        Task<ShowProgress> DeleteWatchedEpisodes(string id, string userLogin, IEnumerable<EpisodeProgress> episodes);
    }
}