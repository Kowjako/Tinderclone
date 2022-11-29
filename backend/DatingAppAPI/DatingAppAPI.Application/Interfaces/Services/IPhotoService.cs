using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace DatingAppAPI.Application.Interfaces.Services
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}
