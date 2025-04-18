﻿using API.DTOs;
using API.DTOs.AuthDtos;
using API.DTOs.OwnerDtos;
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
            var image = new ProfileImageDto
            {
                Url = user.ProfileImage?.Url,
                PublicId = user.ProfileImage?.PublicId,
            };
            var userDto = new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                ExpiresOn = jwtToken.ValidTo,
                IsAuthenticated = true,
                ProfileImage = image,
                DisplayName = user.DisplayName,
            };

            return Ok(userDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await authenticationService.RegisterAsync(model);
            
            if (!result.IsAuthenticated) return Unauthorized(new { Message = result.Message });
            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("getToken")]
        public async Task<ActionResult> GetTokenAsnc(LoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await authenticationService.GetTokenAsync(model);
            if (!result.IsAuthenticated) return  Unauthorized(new {Message = result.Message});

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

        [Authorize(Roles ="Owner")]
        [HttpPut("editProfile")]
        public async Task<IActionResult> EditProfile([FromForm] EditProfileDto dto)
        {
            var id = User.GetUserId();
            var result = await authenticationService.EditProfileAsync(id, dto);
            if(result is null) return Unauthorized();
            return Ok(result);
        }

        [Authorize(Roles = "Owner")]
        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var id = User.GetUserId();
            var result = await authenticationService.ChangePasswordAsync(id, changePasswordDto);
            if(!result) return Unauthorized(new { Message = "Invalid Password"});
            return Ok();
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
