using API.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace API.Helpers.Resolvers
{
    public class UserNameResolver(IHttpContextAccessor httpContextAccessor)
        : IValueResolver<Property, GetPropertiesDto, string>
    {
        public string Resolve(Property source, GetPropertiesDto destination, string destMember, ResolutionContext context)
        {
            //var email = httpContextAccessor.HttpContext!.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email).Value;
            //return userManager.Users.SingleOrDefault(u=>u.Email == email).Id;

            return httpContextAccessor.HttpContext!.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        }
    }
}
