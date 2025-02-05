using API.DTOs;
using API.Models;
using AutoMapper;
using System.Security.Claims;

namespace API.Helpers
{
    public class AppUserIdResolver(IHttpContextAccessor httpContextAccessor) : IValueResolver<PropertyDto, Property, string>
    {
        public string Resolve(PropertyDto source, Property destination, string destMember, ResolutionContext context)
        {
            return httpContextAccessor.HttpContext!.User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
    }
}
