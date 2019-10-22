using System.Collections.Generic;
using System.Linq;
using TvShowTracker.Domain.Models.Show;
using TvShowTracker.Domain.Models.ShowProgress;
using TvShowTracker.Domain.Services;
using Xunit;

namespace TvShowTracker.UnitTests.Domain.Services
{
    public class ShowProgressServiceTests
    {
        private readonly IShowProgressService _service;

        public ShowProgressServiceTests()
        {
            _service = new ShowProgressService();
        }
        
        [Fact]
        public void MergeShowWithProgress_EmptyProgress_FirstEpisodeIsNext()
        {
            // Arrange
            var show = new Show
            {
                Id = "42",
                Seasons = new List<Season>
                {
                    new Season
                    {
                        SeasonNumber = 1,
                        Episodes = new List<Episode>
                        {
                            new Episode {EpisodeNumber = 1},
                            new Episode {EpisodeNumber = 2}
                        }
                    }
                }
            };
            var showProgress = new ShowProgress();
            
            // Act
            var result = _service.MergeShowWithProgress(show, showProgress);

            // Assert
            Assert.Equal(1, result.NextEpisode.EpisodeNumber);
            Assert.Equal(1, result.NextEpisode.SeasonNumber);
        }
        
        [Fact]
        public void MergeShowWithProgress_LastEpisodeInSeasonIsCompleted_NextSeasonFirstEpisodeIsNext()
        {
            // Arrange
            var show = new Show
            {
                Id = "42",
                Seasons = new List<Season>
                {
                    new Season
                    {
                        SeasonNumber = 1,
                        Episodes = new List<Episode>
                        {
                            new Episode {EpisodeNumber = 1},
                            new Episode {EpisodeNumber = 2}
                        }
                    },
                    new Season
                    {
                        SeasonNumber = 2,
                        Episodes = new List<Episode>
                        {
                            new Episode {EpisodeNumber = 1},
                            new Episode {EpisodeNumber = 2}
                        }
                    }
                }
            };
            var showProgress = new ShowProgress
            {
                WatchedEpisodes = new List<EpisodeProgress>
                {
                    new EpisodeProgress {EpisodeNumber = 2, SeasonNumber = 1}
                }
            };
            
            // Act
            var result = _service.MergeShowWithProgress(show, showProgress);

            // Assert
            Assert.Equal(1, result.NextEpisode.EpisodeNumber);
            Assert.Equal(2, result.NextEpisode.SeasonNumber);
        }
        
        [Fact]
        public void MergeShowWithProgress_EpisodeSkipped_ExpectedNextEpisode()
        {
            // Arrange
            var show = new Show
            {
                Id = "42",
                Seasons = new List<Season>
                {
                    new Season
                    {
                        SeasonNumber = 1,
                        Episodes = new List<Episode>
                        {
                            new Episode {EpisodeNumber = 1},
                            new Episode {EpisodeNumber = 2},
                            new Episode {EpisodeNumber = 3},
                            new Episode {EpisodeNumber = 4}
                        }
                    }
                }
            };
            var showProgress = new ShowProgress
            {
                WatchedEpisodes = new List<EpisodeProgress>
                {
                    new EpisodeProgress {EpisodeNumber = 3, SeasonNumber = 1}
                }
            };
            
            // Act
            var result = _service.MergeShowWithProgress(show, showProgress);

            // Assert
            Assert.Equal(4, result.NextEpisode.EpisodeNumber);
            Assert.Equal(1, result.NextEpisode.SeasonNumber);
        }
        
        [Fact]
        public void MergeShowWithProgress_AllEpisodesWatched_IsCompleted()
        {
            // Arrange
            var show = new Show
            {
                Id = "42",
                Seasons = new List<Season>
                {
                    new Season
                    {
                        SeasonNumber = 1,
                        Episodes = new List<Episode>
                        {
                            new Episode {EpisodeNumber = 1, Title = "Episode1"},
                            new Episode {EpisodeNumber = 2, Title = "Episode2"}
                        }
                    }
                }
            };
            var showProgress = new ShowProgress
            {
                WatchedEpisodes = new List<EpisodeProgress>
                {
                    new EpisodeProgress {EpisodeNumber = 1, SeasonNumber = 1},
                    new EpisodeProgress {EpisodeNumber = 2, SeasonNumber = 1}
                }
            };
            
            // Act
            var result = _service.MergeShowWithProgress(show, showProgress);

            // Assert
            Assert.True(result.IsCompleted);
            Assert.True(result.SeasonsWithProgress.First().IsCompleted);
        }
        
        [Fact]
        public void MergeShowWithProgress_NotAllEpisodesWatched_IsNotCompleted()
        {
            // Arrange
            var show = new Show
            {
                Id = "42",
                Seasons = new List<Season>
                {
                    new Season
                    {
                        SeasonNumber = 1,
                        Episodes = new List<Episode>
                        {
                            new Episode {EpisodeNumber = 1, Title = "Episode1"},
                            new Episode {EpisodeNumber = 2, Title = "Episode2"}
                        }
                    }
                }
            };
            var showProgress = new ShowProgress
            {
                WatchedEpisodes = new List<EpisodeProgress>
                {
                    new EpisodeProgress {EpisodeNumber = 1, SeasonNumber = 1}
                }
            };
            
            // Act
            var result = _service.MergeShowWithProgress(show, showProgress);

            // Assert
            Assert.False(result.IsCompleted);
            Assert.False(result.SeasonsWithProgress.First().IsCompleted);
        }
    }
}