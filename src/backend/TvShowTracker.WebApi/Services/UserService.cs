using Microsoft.AspNetCore.Http;

namespace TvShowTracker.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public string UserLogin => _httpContextAccessor.HttpContext.User.Identity.Name;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}