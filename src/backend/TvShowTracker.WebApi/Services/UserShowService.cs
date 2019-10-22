using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvShowTracker.DataAccess.Storage.Dao;
using TvShowTracker.Domain.Models.ShowProgress;
using TvShowTracker.Domain.Services;
using TvShowTracker.WebApi.Dto;

namespace TvShowTracker.WebApi.Services
{
    public class UserShowService : IUserShowService
    {
        private readonly IShowProgressService _showProgressService;
        private readonly IShowDao _showDao;
        private readonly IShowProgressDao _showProgressDao;
        private readonly IOMDbShowImportService _omdbShowImportService;
        private readonly IUserService _userService;

        public UserShowService(
            IShowProgressService showProgressService, 
            IShowDao showDao, 
            IShowProgressDao showProgressDao, 
            IOMDbShowImportService omdbShowImportService, 
            IUserService userService)
        {
            _showProgressService = showProgressService;
            _showDao = showDao;
            _showProgressDao = showProgressDao;
            _omdbShowImportService = omdbShowImportService;
            _userService = userService;
        }

        public async Task<List<ShowDto>> GetUserShows()
        {
            var showProgresses = await _showProgressDao.GetAllByUserLogin(_userService.UserLogin);
            var showWithProgresses = await Task.WhenAll(showProgresses.Select(async showProgress =>
            {
                var show = await _showDao.GetById(showProgress.ShowId);
                return _showProgressService.MergeShowWithProgress(show, showProgress);
            }));

            return showWithProgresses.Select(ShowDto.FromShowWithProgress).ToList();
        }

        public async Task<ShowWithSeasonsDto> GetUserShow(string showId)
        {
            var showProgress = await _showProgressDao.GetByShowId(showId, _userService.UserLogin);
            var show = await _showDao.GetById(showProgress.ShowId);

            var showWithProgress = _showProgressService.MergeShowWithProgress(show, showProgress);

            return ShowWithSeasonsDto.FromShowWithProgress(showWithProgress);
        }

        public async Task MarkEpisodeAsWatched(string showId, int seasonNumber, int episodeNumber)
        {
            var episode = new EpisodeProgress {EpisodeNumber = episodeNumber, SeasonNumber = seasonNumber};
            await _showProgressDao.AddWatchedEpisodes(showId, _userService.UserLogin, new[] {episode});
        }
        
        public async Task MarkEpisodeAsNotWatched(string showId, int seasonNumber, int episodeNumber)
        {
            var episode = new EpisodeProgress {EpisodeNumber = episodeNumber, SeasonNumber = seasonNumber};
            await _showProgressDao.DeleteWatchedEpisodes(showId, _userService.UserLogin, new[] {episode});
        }

        public async Task<ShowDto> CreateUserShow(string imdbId)
        {
            var show = await _showDao.GetById(imdbId);
            if (show == null)
            {
                var showFromOMDb = await _omdbShowImportService.GetFromOMDb(imdbId);
                show = await _showDao.CreateIfNotExists(showFromOMDb);
            }

            var showProgress = await _showProgressDao.InsertIfNotExists(imdbId, _userService.UserLogin);
            var showWithProgress = _showProgressService.MergeShowWithProgress(show, showProgress);

            return ShowDto.FromShowWithProgress(showWithProgress);
        }

        public async Task DeleteUserShow(string showId)
        {
            await _showProgressDao.DeleteByShowId(showId, _userService.UserLogin);
        }
    }
}