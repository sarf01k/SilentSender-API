using System.Security.Claims;
using server.Models;

namespace server.Interfaces
{
    public interface IUserRepository
    {
        public Task<Result> Home(ClaimsPrincipal userClaims);
    }
}