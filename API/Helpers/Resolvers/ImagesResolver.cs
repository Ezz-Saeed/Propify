using API.DTOs.PropertyDtos;
using API.Interfaces;
using API.Models;
using API.Services;
using AutoMapper;
using Humanizer;

namespace API.Helpers.Resolvers
{
    public class ImagesResolver(IImageService imageService) : IValueResolver<AddPropertyDto, Property, ICollection<PropertyImage>>
    {
        public  ICollection<PropertyImage> Resolve(AddPropertyDto source, Property destination, ICollection<PropertyImage> destMember, ResolutionContext context)
        {
            List<PropertyImage> images = new List<PropertyImage>();
            if (source.Images is not null && source.Images.Any())
            {
                
                foreach (var i in source.Images)
                {
                    var uploadResult = imageService.UploadImageAsync(i).Result;
                    //if (uploadResult.Error is not null) return null;
                    PropertyImage image = new()
                    {
                        Url = uploadResult.SecureUrl.AbsoluteUri,
                        PublicId = uploadResult.PublicId,
                    };
                    images.Add(image);
                }
                images[0].IsMain = true;
            }
            return images;
        }
    }
}
