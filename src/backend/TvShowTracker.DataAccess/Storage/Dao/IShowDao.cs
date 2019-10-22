using System.Threading.Tasks;
using TvShowTracker.Domain.Models.Show;

namespace TvShowTracker.DataAccess.Storage.Dao
{
    public interface IShowDao
    {
        Task<Show> CreateIfNotExists(Show show);
        Task<Show> GetById(string id);
    }
}