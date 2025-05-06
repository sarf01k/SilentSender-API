using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using server.Data;
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
                var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return new Result { Success = false, Message = "User Email claim not found." };
                }

                var user = await userManager.FindByEmailAsync(userId);

                if (user == null)
                    return new Result { Success = false, Message = "User not found." };

                var messages = await context.Messages
                    .Where(m => m.RecipientId == user.Id)
                    .ToListAsync();

                return new Result
                {
                    Success = true,
                    Message = new
                    {
                        user.Name,
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
    }
}