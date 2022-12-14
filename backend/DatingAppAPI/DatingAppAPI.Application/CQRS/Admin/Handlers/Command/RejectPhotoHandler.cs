using DatingAppAPI.Application.CQRS.Admin.Requests.Command;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Services;
using MediatR;

namespace DatingAppAPI.Application.CQRS.Admin.Handlers.Command
{
    public class RejectPhotoHandler : IRequestHandler<RejectPhotoRequest, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPhotoService _phService;

        public RejectPhotoHandler(IPhotoService phService, IUnitOfWork uow)
        {
            _uow = uow;
            _phService = phService;
        }

        public async Task<Unit> Handle(RejectPhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = await _uow.PhotoRepository.GetPhotoById(request.PhotoId);

            if (photo.PublicId != null)
            {
                /* Remove from Cloudinary */
                var result = await _phService.DeletePhotoAsync(photo.PublicId);
                if (result.Result.Equals("ok"))
                {
                    _uow.PhotoRepository.DeletePhoto(photo);
                }
            }
            else
            {
                _uow.PhotoRepository.DeletePhoto(photo);
            }

            if (!await _uow.Complete()) throw new HttpException(400, "Fail to change photo status");
            return Unit.Value;
        }
    }
}
