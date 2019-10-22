using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TvShowTracker.Domain.Models.ShowProgress
{
    public class ShowProgress
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserLogin { get; set; }
        public string ShowId { get; set; }
        public List<EpisodeProgress> WatchedEpisodes { get; set; }
    }
}