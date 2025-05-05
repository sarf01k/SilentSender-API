using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Interfaces;
using server.Models;
using server.Repository;

namespace server.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IAuthRepository authRepository;

        public AuthController(UserManager<User> userManager, IAuthRepository authRepository)
        {
            this.userManager = userManager;
            this.authRepository = authRepository;
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
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return Unauthorized();

            var email = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)!.Value;
            var name = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Name)!.Value;
            var googleUserId = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;
            var username = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.GivenName)!.Value.ToLower();

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            // {
            //     if (user.UserName == username)
            //     {

            //     }
            //     ;
            // }
            // else
            {
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

            // var claims = result.Principal!.Identities.FirstOrDefault()!.Claims.Select(claim => new
            // {
            //     claim.Type,
            //     claim.Value
            // });

            return Ok(new
            {
                success = true,
                message = "Log in successful.",
            });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await authRepository.LogoutAsync();

            return Ok(new
            {
                success = true,
                message = "Log out successful,"
            });
        }
    }
}