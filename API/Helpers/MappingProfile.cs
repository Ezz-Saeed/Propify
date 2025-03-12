using API.DTOs.OwnerDtos;
using API.DTOs.PropertyDtos;
using API.Helpers.Resolvers;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddPropertyDto, Property>()
                .ForMember(d=>d.Images, opt=>opt.MapFrom<ImagesResolver>())
                .ForMember(d=>d.IsAvailable, opt=>opt.MapFrom(x=>true));

            //CreateMap<AppUser, AuthDto>()
            //    .ForMember(d => d.Roles, opt => opt.MapFrom<UserRolesResolver>());

            CreateMap<Image, ImageDto>();

            CreateMap<PropertyType, TypeDto>()
                .ForMember(d=>d.CategoryName, opt=>opt.MapFrom(s=>s.Category.Name))
                .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.Category.Id));

            CreateMap<UpdatePropertyDto, Property>();

            CreateMap<Property, GetPropertiesDto>()
                .ForMember(d => d.TypeName, opt => opt.MapFrom(s => s.Type.Name))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Type.Category.Name))
                .ForMember(d => d.OwnerName, opt => opt.MapFrom<UserNameResolver>());
                //.ForMember(d => d.TypeId, opt => opt.MapFrom(s=>s.TypeId));
                
        }
    }
}
