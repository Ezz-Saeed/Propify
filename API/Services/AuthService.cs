using API.DTOs.AuthDtos;
using API.DTOs.OwnerDtos;
using API.Helpers;
using API.Interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Services
{
    public class AuthService(UserManager<AppUser> userManager, IOptions<JWT> jwtOptions, 
        RoleManager<IdentityRole> roleManager, IImageService imageService, IMapper mapper) : IAuthService
    {
        private readonly JWT jwt = jwtOptions.Value;

        public async Task<AuthDto> RegisterAsync(RegisterDto model)
        {
            if (await userManager.FindByEmailAsync(model.Email) is not null ||
                await userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthDto { Message = "Already registered account!" };
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                    errors += $"{error.Description}, ";
                return new AuthDto { Message = errors };
            }

            await userManager.AddToRoleAsync(user, "User");
            await userManager.AddToRoleAsync(user, "Owner");
            var token = await GenerateToken(user);
            return new AuthDto
            {
                Email = user.Email,
                Username = user.Email,
                Roles = new List<string> { "User", "Owner" },
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresOn = token.ValidTo,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

        }


        public async Task<AuthDto> GetTokenAsync(LoginDto model)
        {
            var authDto = new AuthDto();
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                authDto.Message = "Invalid email or password!";
                return authDto;
            }
            var token = await GenerateToken(user);
            var roles = await userManager.GetRolesAsync(user);

            authDto.Username = user.UserName;
            authDto.Email = user.Email;
            authDto.Roles = roles.ToList();
            authDto.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authDto.ExpiresOn = token.ValidTo.ToLocalTime();
            authDto.FirstName = user.FirstName;
            authDto.LastName = user.LastName;
            authDto.IsAuthenticated = true;
            authDto.ProfileImage.Url = user.ProfileImage?.Url;
            authDto.ProfileImage.PublicId = user.ProfileImage?.PublicId;

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authDto.RefreshToken = activeRefreshToken.Token;
                authDto.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authDto.RefreshToken = refreshToken.Token;
                authDto.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await userManager.UpdateAsync(user);
            }
;
            return authDto;
        }


        public async Task<AuthDto> RefreshTokenAsync(string refreshToken)
        {
            var authModel = new AuthDto();
            var user = await userManager.Users.SingleOrDefaultAsync
                (u => u.RefreshTokens.Any(rt => rt.Token == refreshToken));
            if (user is null)
            {
                authModel.Message = "Invalid refresh token!";
                return authModel;
            }
            var activeRefreshToken = user.RefreshTokens.Single(rt => rt.Token == refreshToken);

            if (!activeRefreshToken.IsActive)
            {
                authModel.Message = "Inactive refresh token!";
                return authModel;
            }
            activeRefreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);

            var jwtToken = await GenerateToken(user);
            var roles = await userManager.GetRolesAsync(user);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.Roles = roles.ToList();
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
            return authModel;
        }

        public async Task<OwnerDto> EditProfileAsync(string id, EditProfileDto dto)
        {
            var owner = await userManager.FindByIdAsync(id);
            if (owner is null) return null;
            if(dto.FirstName is not null)
                owner.FirstName = dto.FirstName;
            if (dto.LastName is not null)
                owner.LastName = dto.LastName;
            if(dto.DisplayName is not null)
                owner.DisplayName = dto.DisplayName;
            if(dto.ProfileImage is not null && dto.ProfileImage.Length > 0)
            {
                if(owner.ProfileImage is not null && !string.IsNullOrEmpty(owner.ProfileImage.PublicId))
                {
                    var deleteResult = await imageService.DeleteImageAsync(owner.ProfileImage.PublicId);
                    owner.ProfileImage = null;
                }
                var result = await imageService.UploadImageAsync(dto.ProfileImage);
                ProfileImage image = new()
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId,
                    AppUserId = owner.Id,
                };
                owner.ProfileImage = image;
            }
            await userManager.UpdateAsync(owner);
            var updatedOwner = await userManager.FindByIdAsync(id);
            var ownerToReturn = mapper.Map<OwnerDto>(owner);
            return ownerToReturn;
        }

        public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
        {
            var user = await userManager.FindByIdAsync(userId);
            if(user is null) return false;
            var isPasswordValid = await userManager.CheckPasswordAsync(user, changePasswordDto.CurrentPassword);
            if(!isPasswordValid) return false;
            var result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword,changePasswordDto.NewPassword);
            return result.Succeeded;
        }

        public async Task<AppUser> LoadCurrentUser(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null!;
            var user = await userManager.FindByIdAsync(userId);
            if (user is null) return null!;
            return user;
        }

        public async Task<JwtSecurityToken> GenerateToken(AppUser appUser)
        {
            var userClaims = await userManager.GetClaimsAsync(appUser);
            var roles = await userManager.GetRolesAsync(appUser);
            var roleClaims = new List<Claim>();
            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, appUser.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email!),
                new Claim(JwtRegisteredClaimNames.NameId, appUser.Id)
            }.Union(userClaims).Union(roleClaims);

            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                    issuer: jwt.Issuer,
                    audience: jwt.Audience,
                    claims: claims,
                    signingCredentials: signinCredentials,
                    expires: DateTime.Now.AddDays(jwt.DurationInMinutes)
                );
        }


        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddDays(3),
            };
        }

        
    }
}
