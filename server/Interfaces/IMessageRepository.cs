using server.DTO;
using server.Models;

namespace server.Interfaces
{
    public interface IMessageRepository
    {
        Task<Result> SubmitMessage(string username, SubmitMessageDTO submitMessageDTO);
        Task<Result> DeleteMessage(Guid Id);
    }
}