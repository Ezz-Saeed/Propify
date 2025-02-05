using API.DTOs;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddPropertyDto, Property>();
            CreateMap<Property, GetPropertiesDto>()
                .ForMember(d=>d.TypeName, opt=>opt.MapFrom(s=>s.Type.Name))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(s => s.Type.Category.Name))
                .ForMember(d => d.OwnerName, opt => opt.MapFrom<UserNameResolver>());
        }
    }
}
