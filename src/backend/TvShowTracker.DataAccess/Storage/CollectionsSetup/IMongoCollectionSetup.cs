using System.Threading.Tasks;

namespace TvShowTracker.DataAccess.Storage.CollectionsSetup
{
    public interface IMongoCollectionSetup
    {
        Task Setup();
    }
}