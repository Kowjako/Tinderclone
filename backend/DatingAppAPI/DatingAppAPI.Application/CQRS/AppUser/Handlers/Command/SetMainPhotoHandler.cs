using DatingAppAPI.Application.CQRS.AppUser.Requests.Command;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Common;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Handlers.Command
{
    public class SetMainPhotoHandler : IRequestHandler<SetMainPhotoRequest, bool>
    {
        private readonly IUnitOfWork _uow;

        public SetMainPhotoHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<bool> Handle(SetMainPhotoRequest request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(request.Username);

            if (user == null) throw new HttpException(404, string.Empty);

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.PhotoId);
            if (photo == null) throw new HttpException(404, string.Empty);

            if (photo.IsMain) throw new HttpException(400, "This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            return await _uow.Complete();
        }
    }
}
