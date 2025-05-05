using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using server.Interfaces;

namespace server.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthRepository(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task LogoutAsync()
        {
            await httpContextAccessor.HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}