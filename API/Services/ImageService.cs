using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary cloudinary;
        public ImageService(IOptions<CloudinarySettings> options)
        {
            Account account = new Account(options.Value.CloudName,options.Value.ApiKey,options.Value.ApiSecret);
            cloudinary = new Cloudinary(account);
        }
        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return await cloudinary.DestroyAsync(deleteParams);
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile image)
        {
            var uploadResult = new ImageUploadResult();

            if(image is not null && image.Length > 0)
            {
                using var stream = image.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(image.Name, stream),
                    Transformation = new Transformation().Width(400).Height(400).Crop("fill"),
                };

                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }
    }
}
