﻿using API.DTOs.AuthDtos;
using API.Models;
using System.IdentityModel.Tokens.Jwt;

namespace API.Interfaces
{
    public interface IAuthService
    {
        Task<AuthDto> RegisterAsync(RegisterDto dto);
        Task<AuthDto> GetTokenAsync(LoginDto dto);
        Task<AuthDto> RefreshTokenAsync(string refreshToken);
        Task<AppUser> LoadCurrentUser(string userId);
        Task<JwtSecurityToken> GenerateToken(AppUser appUser);
        //Task<bool> RevokRefreshToken(string refreshToken);
    }
}
