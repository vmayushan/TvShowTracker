using System.Threading.Tasks;
using MongoDB.Driver;
using TvShowTracker.Domain.Models.ShowProgress;

namespace TvShowTracker.DataAccess.Storage.CollectionsSetup
{
    public class ShowProgressCollectionSetup : IMongoCollectionSetup
    {
        private readonly IMongoCollection<ShowProgress> _collection;

        public ShowProgressCollectionSetup(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<ShowProgress>(nameof(ShowProgress));
        }
        
        public async Task Setup()
        {
            await _collection
                .Indexes
                .CreateOneAsync(
                    new CreateIndexModel<ShowProgress>(
                        Builders<ShowProgress>.IndexKeys
                            .Ascending(x => x.UserLogin)
                            .Ascending(x => x.ShowId),
                        new CreateIndexOptions
                        {
                            Unique = true,
                            Background = true
                        }
                    )
                );
        }
    }
}