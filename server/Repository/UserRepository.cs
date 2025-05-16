using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTO;
using server.Interfaces;
using server.Models;

namespace server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<MessageRepository> logger;
        private readonly ApplicationDBContext context;
        private readonly UserManager<User> userManager;

        public UserRepository(ILogger<MessageRepository> logger, ApplicationDBContext context, UserManager<User> userManager)
        {
            this.logger = logger;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Result> Home(ClaimsPrincipal userClaims)
        {
            try
            {
                var userEmail = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var user = await userManager.FindByEmailAsync(userEmail!);

                var messages = await context.Messages
                    .Where(m => m.RecipientId == user!.Id)
                    .Select(m => new
                    {
                        m.Id,
                        m.Content,
                        m.Sender,
                        m.Tag
                    })
                    .ToListAsync();

                return new Result
                {
                    Success = true,
                    Message = new
                    {
                        user!.Name,
                        user.UserName,
                        messages
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occured.");
                return new Result { Success = false, Message = "An unexpected error occurred." };
            }
        }

        public async Task<Result> GetUser(string username)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    return new Result { Success = false, Message = "User not found." };
                }

                return new Result
                {
                    Success = true,
                    Message = new
                    {
                        user!.Name,
                        user.UserName,
                        user.Note
                    }
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occured.");
                return new Result { Success = false, Message = "An unexpected error occurred." };
            }
        }

        public async Task<Result> ChangeName(ClaimsPrincipal userClaims, NameDTO nameDTO)
        {
            try
            {
                var userEmail = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var user = await userManager.FindByEmailAsync(userEmail!);

                if (user == null)
                {
                    return new Result { Success = false, Message = "User not found." };
                }

                user.Name = nameDTO.Name;
                await context.SaveChangesAsync();

                return new Result
                {
                    Success = true,
                    Message = "Your name has been changed."
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occured.");
                return new Result { Success = false, Message = "An unexpected error occurred." };
            }
        }

        public async Task<Result> ChangeUsername(ClaimsPrincipal userClaims, UsernameDTO usernameDTO)
        {
            try
            {
                var userEmail = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var user = await userManager.FindByEmailAsync(userEmail!);

                if (user == null)
                {
                    return new Result { Success = false, Message = "User not found." };
                }

                var existingUser = await userManager.FindByNameAsync(usernameDTO.Username);

                if (existingUser != null)
                {
                    return new Result { Success = false, Message = "This username is taken." };
                }

                user.UserName = usernameDTO.Username;
                await context.SaveChangesAsync();

                return new Result
                {
                    Success = true,
                    Message = "Your username has been changed."
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occured.");
                return new Result { Success = false, Message = "An unexpected error occurred." };
            }
        }

        public async Task<Result> DeleteAccount(ClaimsPrincipal userClaims, HttpContext httpContext)
        {
            try
            {

                var userEmail = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                var user = await userManager.FindByEmailAsync(userEmail!);

                if (user == null)
                {
                    return new Result { Success = false, Message = "User not found." };
                }

                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                context.Remove(user);
                await context.SaveChangesAsync();

                return new Result
                {
                    Success = true,
                    Message = "Your account has been deleted."
                };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occured.");
                return new Result { Success = false, Message = "An unexpected error occurred." };
            }
        }
    }
}