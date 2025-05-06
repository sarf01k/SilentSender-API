using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.DTO;
using server.Interfaces;

namespace server.Controllers
{
    [ApiController]
    [Route("api")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository messageRepository;

        public MessageController(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> SubmitMessage([FromRoute] string username, [FromBody] SubmitMessageDTO submitMessageDTO)
        {
            try
            {
                var result = await messageRepository.SubmitMessage(username, submitMessageDTO);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal Server Error."
                });
            }
        }

        [HttpDelete("messages")]
        public async Task<IActionResult> DeleteMessage([FromQuery] Guid id)
        {
            try
            {
                var result = await messageRepository.DeleteMessage(id);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Internal Server Error."
                });
            }
        }
    }
}