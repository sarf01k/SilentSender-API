using Microsoft.AspNetCore.Identity;
using server.Data;
using server.DTO;
using server.Interfaces;
using server.Models;

namespace server.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ILogger<MessageRepository> logger;
        private readonly ApplicationDBContext context;
        private readonly UserManager<User> userManager;

        public MessageRepository(ILogger<MessageRepository> logger, ApplicationDBContext context, UserManager<User> userManager)
        {
            this.logger = logger;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<Result> SubmitMessage(string username, SubmitMessageDTO submitMessageDTO)
        {
            try
            {
                var recipient = await userManager.FindByNameAsync(username);

                var message = new Message
                {
                    Content = submitMessageDTO.Content,
                    RecipientId = recipient!.Id,
                    Sender = submitMessageDTO.Sender,
                    Tag = (Models.Tag?)submitMessageDTO.Tag,
                    IsAnonymous = submitMessageDTO.IsAnonymous,
                    Flagged = submitMessageDTO.Flagged
                };

                await context.Messages.AddAsync(message);
                await context.SaveChangesAsync();

                return new Result { Success = true, Message = "Message submitted successfully." };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
                return new Result { Success = false, Message = "An unexpected error occurred." };
            }
        }

        public async Task<Result> DeleteMessage(Guid id)
        {
            try
            {
                var message = await context.Messages.FindAsync(id);

                if (message == null)
                    return new Result { Success = false, Message = "Message not found." };

                context.Remove(message);
                await context.SaveChangesAsync();

                return new Result { Success = true, Message = "Message deleted successfully." };
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred.");
                return new Result { Success = false, Message = "An unexpected error occurred." };
            }
        }
    }
}