using System.Threading.Tasks;
using MongoDB.Driver;
using TvShowTracker.Domain.Models.Show;

namespace TvShowTracker.DataAccess.Storage.CollectionsSetup
{
    public class ShowCollectionSetup : IMongoCollectionSetup
    {
        private readonly IMongoCollection<Show> _collection;

        public ShowCollectionSetup(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<Show>(nameof(Show));
        }

        public Task Setup()
        {
            return Task.CompletedTask;
        }
    }
}