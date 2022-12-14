using DatingAppAPI.Application.CQRS.AppUser.Requests.Command;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Services;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Handlers.Command
{
    public class DeletePhotoHandler : IRequestHandler<DeletePhotoRequest, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPhotoService _photoService;

        public DeletePhotoHandler(IPhotoService photoService, IUnitOfWork uow)
        {
            _uow = uow;
            _photoService = photoService;
        }

        public async Task<Unit> Handle(DeletePhotoRequest request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(request.Username);

            if (user == null) throw new HttpException(404, string.Empty);

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
            if (photo == null) throw new HttpException(404, string.Empty);

            if (photo.IsMain) throw new HttpException(400, "You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) throw new HttpException(400, result.Error.Message);
            }

            user.Photos.Remove(photo);
            if (!await _uow.Complete())
            {
                throw new HttpException(400, "Problem deleting photo");
            }
            return Unit.Value;
        }
    }
}
