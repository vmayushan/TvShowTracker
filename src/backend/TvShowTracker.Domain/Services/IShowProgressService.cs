using TvShowTracker.Domain.Models.Show;
using TvShowTracker.Domain.Models.ShowProgress;
using TvShowTracker.Domain.Models.ShowWithProgress;

namespace TvShowTracker.Domain.Services
{
    public interface IShowProgressService
    {
        ShowWithProgress MergeShowWithProgress(Show show, ShowProgress progress);
    }
}