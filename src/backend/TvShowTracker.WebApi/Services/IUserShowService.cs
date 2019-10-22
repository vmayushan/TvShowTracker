using System.Collections.Generic;
using System.Threading.Tasks;
using TvShowTracker.WebApi.Dto;

namespace TvShowTracker.WebApi.Services
{
    public interface IUserShowService
    {
        Task<ShowWithSeasonsDto> GetUserShow(string showId);
        Task<List<ShowDto>> GetUserShows();
        Task MarkEpisodeAsWatched(string showId, int seasonNumber, int episodeNumber);
        Task MarkEpisodeAsNotWatched(string showId, int seasonNumber, int episodeNumber);
        Task DeleteUserShow(string showId);
        Task<ShowDto> CreateUserShow(string imdbId);
    }
}