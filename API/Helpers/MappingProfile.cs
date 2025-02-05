﻿using API.DTOs;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PropertyDto, Property>().
                ForMember(d=>d.AppUserId, options=>options.MapFrom<AppUserIdResolver>());
        }
    }
}
