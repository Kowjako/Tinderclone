using DatingAppAPI.Application.DTO;
using DatingAppAPI.Domain.Entities;

namespace DatingAppAPI.Application.Interfaces.Repositories
{
    public interface IPhotoRepository
    {
        Task<List<PhotoForApprovalDTO>> GetUnapprovedPhotos();
        Task<Photo> GetPhotoById(int id);
        void DeletePhoto(Photo photo);
    }
}
