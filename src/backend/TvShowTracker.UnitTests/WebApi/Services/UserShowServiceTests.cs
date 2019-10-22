using System.Threading.Tasks;
using Moq;
using TvShowTracker.DataAccess.Storage.Dao;
using TvShowTracker.Domain.Models.Show;
using TvShowTracker.Domain.Services;
using TvShowTracker.WebApi.Services;
using Xunit;

namespace TvShowTracker.UnitTests.WebApi.Services
{
    public class UserShowServiceTests
    {
        private readonly IUserShowService _service;
        private readonly Mock<IShowProgressService> _showProgressServiceMock;
        private readonly Mock<IShowDao> _showDaoMock;
        private readonly Mock<IShowProgressDao> _showProgressDaoMock;
        private readonly Mock<IOMDbShowImportService> _omdbShowImportServiceMock;
        private readonly Mock<IUserService> _userServiceMock;

        /*
         * TODO tests for remaining methods
         */

        public UserShowServiceTests()
        {
            _showDaoMock = new Mock<IShowDao>();
            _showProgressDaoMock = new Mock<IShowProgressDao>();
            _omdbShowImportServiceMock = new Mock<IOMDbShowImportService>();
            _showProgressServiceMock = new Mock<IShowProgressService>();
            _userServiceMock = new Mock<IUserService>();
            
            _service = new UserShowService(
                _showProgressServiceMock.Object, 
                _showDaoMock.Object, 
                _showProgressDaoMock.Object,
                _omdbShowImportServiceMock.Object,
                _userServiceMock.Object);
        }

        [Fact]
        public async Task CreateUserShow_ShowNotExistsInDb_ShouldCreateShow()
        {
            // Arrange
            const string id = "42";
            var show = new Show();

            _omdbShowImportServiceMock
                .Setup(x => x.GetFromOMDb(id))
                .ReturnsAsync(show);
            
            // Act
            await _service.CreateUserShow(id);

            // Assert
            _omdbShowImportServiceMock.Verify(x => x.GetFromOMDb(id), Times.Once);
            _showDaoMock.Verify(x => x.CreateIfNotExists(show), Times.Once);
        }
        
        [Fact]
        public async Task CreateUserShow_ShowExistsInDb_ShouldNotCreateShow()
        {
            // Arrange
            const string id = "42";
            var show = new Show();

            _showDaoMock
                .Setup(x => x.GetById(id))
                .ReturnsAsync(show);
            
            // Act
            await _service.CreateUserShow(id);

            // Assert
            _omdbShowImportServiceMock.Verify(x => x.GetFromOMDb(id), Times.Never);
            _showDaoMock.Verify(x => x.CreateIfNotExists(show), Times.Never);
        }
    }
}