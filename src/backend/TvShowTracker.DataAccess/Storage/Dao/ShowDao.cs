using System.Threading.Tasks;
using MongoDB.Driver;
using TvShowTracker.Domain.Models.Show;

namespace TvShowTracker.DataAccess.Storage.Dao
{
    public class ShowDao : IShowDao
    {
        private readonly IMongoCollection<Show> _collection;

        public ShowDao(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<Show>(nameof(Show));
        }

        public Task<Show> CreateIfNotExists(Show show)
        {
            return _collection.FindOneAndUpdateAsync(
                Builders<Show>.Filter.Eq(x => x.Id, show.Id),
                Builders<Show>.Update
                    .Set(x => x.Title, show.Title)
                    .Set(x => x.Plot, show.Plot)
                    .Set(x => x.Poster, show.Poster)
                    .Set(x => x.Seasons, show.Seasons),
                new FindOneAndUpdateOptions<Show>
                {
                    IsUpsert = true,
                    ReturnDocument = ReturnDocument.After
                }
            );
        }

        public Task<Show> GetById(string id)
        {
            return _collection.Find(
                Builders<Show>.Filter.Eq(x => x.Id, id)
            ).SingleOrDefaultAsync();
        }
    }
}