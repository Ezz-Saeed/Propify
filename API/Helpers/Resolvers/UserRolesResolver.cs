//using API.DTOs;
//using API.Models;
//using AutoMapper;

//namespace API.Helpers.Resolvers
//{
//    public class UserRolesResolver(HttpContextAccessor httpContextAccessor) : IValueResolver<AppUser, AuthDto, List<string>>
//    {
//        public List<string> Resolve(AppUser source, AuthDto destination, List<string> destMember, ResolutionContext context)
//        {
//            var roles = httpContextAccessor.HttpContext!.User.Claims.Where(c => c.Type == "roles");
//            List<string> result = new List<string>();
//            foreach (var role in roles)
//            {
//                result.Add(role.Value);
//            }
//            return result;
//        }
//    }
//}
