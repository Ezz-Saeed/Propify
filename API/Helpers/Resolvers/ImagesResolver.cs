using API.DTOs;
using API.Interfaces;
using API.Models;
using API.Services;
using AutoMapper;
using Humanizer;

namespace API.Helpers.Resolvers
{
    public class ImagesResolver(IImageService imageService) : IValueResolver<AddPropertyDto, Property, ICollection<Image>>
    {
        public  ICollection<Image> Resolve(AddPropertyDto source, Property destination, ICollection<Image> destMember, ResolutionContext context)
        {
            List<Image> images = new List<Image>();
            if (source.Images is not null && source.Images.Any())
            {
                
                foreach (var i in source.Images)
                {
                    var uploadResult = imageService.UploadImageAsync(i).Result;
                    //if (uploadResult.Error is not null) return null;
                    Image image = new()
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
