using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using TvShowTracker.Domain.Models.ShowProgress;

namespace TvShowTracker.DataAccess.Storage.Dao
{
    public class ShowProgressDao : IShowProgressDao
    {
        private readonly IMongoCollection<ShowProgress> _collection;

        public ShowProgressDao(IMongoDatabase mongoDatabase)
        {
            _collection = mongoDatabase.GetCollection<ShowProgress>(nameof(ShowProgress));
        }
        
        public Task<ShowProgress> InsertIfNotExists(string showId, string userLogin)
        {
            return _collection.FindOneAndUpdateAsync(
                Builders<ShowProgress>.Filter.And(
                        Builders<ShowProgress>.Filter.Eq(x => x.ShowId, showId),
                        Builders<ShowProgress>.Filter.Eq(x => x.UserLogin, userLogin)
                    ),
                Builders<ShowProgress>.Update
                    .SetOnInsert(x => x.UserLogin, userLogin)
                    .SetOnInsert(x => x.ShowId, showId),
                new FindOneAndUpdateOptions<ShowProgress>
                {
                    IsUpsert = true,
                    ReturnDocument = ReturnDocument.After
                }
            );
        }

        public Task DeleteByShowId(string showId, string userLogin)
        {
            return _collection.FindOneAndDeleteAsync(
                Builders<ShowProgress>.Filter.And(
                    Builders<ShowProgress>.Filter.Eq(x => x.ShowId, showId),
                    Builders<ShowProgress>.Filter.Eq(x => x.UserLogin, userLogin)
                )
            );
        }
        
        public Task<ShowProgress> GetByShowId(string showId, string userLogin)
        {
            return _collection.Find(
                Builders<ShowProgress>.Filter.And(
                    Builders<ShowProgress>.Filter.Eq(x => x.ShowId, showId),
                    Builders<ShowProgress>.Filter.Eq(x => x.UserLogin, userLogin)
                )
            ).SingleOrDefaultAsync();
        }

        public Task<List<ShowProgress>> GetAllByUserLogin(string userLogin)
        {
            return _collection.Find(
                Builders<ShowProgress>.Filter.Eq(x => x.UserLogin, userLogin)
            ).ToListAsync();
        }
        
        public Task<ShowProgress> AddWatchedEpisodes(string showId, string userLogin, IEnumerable<EpisodeProgress> episodes)
        {
            return _collection.FindOneAndUpdateAsync(
                Builders<ShowProgress>.Filter.And(
                    Builders<ShowProgress>.Filter.Eq(x => x.ShowId, showId),
                    Builders<ShowProgress>.Filter.Eq(x => x.UserLogin, userLogin)
                ),
                Builders<ShowProgress>.Update
                    .AddToSetEach(x => x.WatchedEpisodes, episodes)
            );
        }
        
        public Task<ShowProgress> DeleteWatchedEpisodes(string showId, string userLogin, IEnumerable<EpisodeProgress> episodes)
        {
            return _collection.FindOneAndUpdateAsync(
                Builders<ShowProgress>.Filter.And(
                    Builders<ShowProgress>.Filter.Eq(x => x.ShowId, showId),
                    Builders<ShowProgress>.Filter.Eq(x => x.UserLogin, userLogin)
                ),
                Builders<ShowProgress>.Update
                    .PullAll(x => x.WatchedEpisodes, episodes)
            );
        }
    }
}