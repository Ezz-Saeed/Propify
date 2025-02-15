using CloudinaryDotNet.Actions;

namespace API.Interfaces
{
    public interface IImageService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile image);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
