using System.Security.Claims;
using server.DTO;
using server.Models;

namespace server.Interfaces
{
    public interface IUserRepository
    {
        public Task<Result> Home(ClaimsPrincipal userClaims);
        public Task<Result> GetUser(string username);
        public Task<Result> ChangeName(ClaimsPrincipal userClaims, NameDTO nameDTO);
        public Task<Result> ChangeUsername(ClaimsPrincipal userClaims, UsernameDTO usernameDTO);
        public Task<Result> DeleteAccount(ClaimsPrincipal userClaims, HttpContext httpContext);
    }
}