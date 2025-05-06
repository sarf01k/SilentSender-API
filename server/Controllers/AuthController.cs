using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Models;

namespace server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public AuthController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet("google")]
        public IActionResult GoogleSignIn()
        {
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleCallback")
            };

            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()   // user is stored in database here
        {
            try
            {
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (!result.Succeeded)
                    return Unauthorized();

                var email = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)!.Value;

                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    var name = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Name)!.Value;
                    var googleUserId = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
                    var baseUsername = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.GivenName)!.Value.ToLower();

                    var username = baseUsername;
                    int suffix = 0;

                    while (await userManager.FindByNameAsync(username) != null)
                    {
                        suffix = new Random().Next(1000, 9999);
                        username = $"{baseUsername}-{suffix}";
                    }

                    user = new User
                    {
                        Email = email,
                        Name = name,
                        GoogleUserId = googleUserId,
                        UserName = username
                    };

                    var createdUser = await userManager.CreateAsync(user);

                    if (!createdUser.Succeeded)
                        return BadRequest(createdUser.Errors);
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Log in successful.",
                });
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

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                return Ok(new
                {
                    Success = true,
                    Message = "Log out successful."
                });
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