using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces;

namespace server.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Home()
        {
            try
            {
                var result = await userRepository.Home(User);

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