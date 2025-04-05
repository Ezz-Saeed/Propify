using API.DTOs.OwnerDtos;
using API.Models;
using AutoMapper;
using AutoMapper.Execution;

namespace API.Helpers.Resolvers
{
    public class ProfileImageResolver : IValueResolver<AppUser, OwnerDto, ProfileImageDto>
    {
        public ProfileImageDto Resolve(AppUser source, OwnerDto destination, ProfileImageDto destMember, ResolutionContext context)
        {
            var profileImage = new ProfileImageDto();
            if (source.ProfileImage is not null)
            {
                profileImage.PublicId = source.ProfileImage.PublicId;
                profileImage.Url = source.ProfileImage.Url;
            }
            return profileImage;
        }
    }
}
