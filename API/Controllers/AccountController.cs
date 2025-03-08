using API.DTOs;
using API.Extensions;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthService authenticationService, UserManager<AppUser> userManager) : ControllerBase
    {

        [HttpGet("loadCurrentUser")]
        [Authorize]
        public async Task<IActionResult> LoadCurrentUser()
        {
            var user = await authenticationService.LoadCurrentUser(User.GetUserId());
            if(user is null) return Unauthorized("Unauthorized user!");
            var roles = await userManager.GetRolesAsync(user);
            var jwtToken = await authenticationService.GenerateToken(user);
            var userDto = new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                ExpiresOn = jwtToken.ValidTo,
                IsAuthenticated = true
            };

            return Ok(userDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await authenticationService.RegisterAsync(model);

            if (!result.IsAuthenticated) return Ok(result);
            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("getToken")]
        public async Task<ActionResult> GetTokenAsnc(LoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await authenticationService.GetTokenAsync(model);
            if (!result.IsAuthenticated) return  Ok(result);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await authenticationService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);
            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                SameSite = SameSiteMode.Strict,
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
