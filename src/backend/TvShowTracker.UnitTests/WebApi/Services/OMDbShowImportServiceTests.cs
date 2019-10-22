using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using TvShowTracker.DataAccess.OMDb;
using TvShowTracker.DataAccess.OMDb.Models;
using TvShowTracker.WebApi.Services;
using Xunit;

namespace TvShowTracker.UnitTests.WebApi.Services
{
    public class OMDbShowImportServiceTests
    {
        private readonly IOMDbShowImportService _service;
        private readonly Mock<IOMDbShowClient> _omdbShowClientMock;
        private readonly Mock<ILogger<IOMDbShowImportService>> _loggerMock;

        public OMDbShowImportServiceTests()
        {
            _omdbShowClientMock = new Mock<IOMDbShowClient>();
            _loggerMock = new Mock<ILogger<IOMDbShowImportService>>();
            _service = new OMDbShowImportService(
                _omdbShowClientMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task GetFromOMDb_ThreeSeasons_ShouldFetchAllSeasons()
        {
            // Arrange
            const string imdbId = "42";
            _omdbShowClientMock
                .Setup(x => x.GetShowByImdbId(imdbId))
                .ReturnsAsync(new OMDbShow { TotalSeasons = "3" });

            _omdbShowClientMock
                .Setup(x => x.GetSeason(imdbId, It.IsAny<int>()))
                .ReturnsAsync(new OMDbSeason {Episodes = new List<OMDbEpisode>()});
            
            // Act
            var result = await _service.GetFromOMDb(imdbId);

            // Assert
            _omdbShowClientMock.Verify(x => x.GetSeason(imdbId, It.IsAny<int>()), Times.Exactly(3));
            Assert.Equal(3, result.Seasons.Count);
        }
    }
}