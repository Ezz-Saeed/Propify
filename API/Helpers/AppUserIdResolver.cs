using API.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace API.Helpers
{
    public class AppUserIdResolver(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager) : IValueResolver<PropertyDto, Property, string>
    {
        public string Resolve(PropertyDto source, Property destination, string destMember, ResolutionContext context)
        {
            var email = httpContextAccessor.HttpContext!.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value;
            return userManager.Users.SingleOrDefault(u=>u.Email == email).Id;
        }
    }
}
