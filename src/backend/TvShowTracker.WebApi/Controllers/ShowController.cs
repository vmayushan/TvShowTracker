using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TvShowTracker.DataAccess.OMDb.Models;
using TvShowTracker.WebApi.Dto;
using TvShowTracker.WebApi.Services;

namespace TvShowTracker.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ShowController : ControllerBase
    {
        private readonly IUserShowService _userShowService;
        private readonly IOMDbShowImportService _omdbShowImportService;

        public ShowController(
            IUserShowService userShowService, 
            IOMDbShowImportService omdbShowImportService)
        {
            _userShowService = userShowService;
            _omdbShowImportService = omdbShowImportService;
        }

        [HttpGet]
        public Task<List<ShowDto>> GetAll()
        {
            return _userShowService.GetUserShows();
        }

        [HttpGet("{id}")]
        public Task<ShowWithSeasonsDto> GetById([FromRoute]string id)
        {
            return _userShowService.GetUserShow(id);
        }
        
        [HttpDelete("{id}")]
        public Task DeleteById([FromRoute]string id)
        {
            return _userShowService.DeleteUserShow(id);
        }

        [HttpPost]
        public Task<ShowDto> CreateByImdbId([FromQuery]string imdbId)
        {
            return _userShowService.CreateUserShow(imdbId);
        }

        [HttpPut("{id}/season/{seasonNumber}/episode/{episodeNumber}/watched")]
        public Task MarkAsWatched([FromRoute]string id, [FromRoute]int seasonNumber, [FromRoute]int episodeNumber)
        {
            return _userShowService.MarkEpisodeAsWatched(id, seasonNumber, episodeNumber);
        }
        
        [HttpPut("{id}/season/{seasonNumber}/episode/{episodeNumber}/not-watched")]
        public Task MarkAsNotWatched([FromRoute]string id, [FromRoute]int seasonNumber, [FromRoute]int episodeNumber)
        {
            return _userShowService.MarkEpisodeAsNotWatched(id, seasonNumber, episodeNumber);
        }

        [HttpGet("search")]
        public Task<List<OMDbShowAutocomplete>> SearchByTitle([FromQuery]string query)
        {
            return _omdbShowImportService.SearchByTitle(query);
        }
    }
}