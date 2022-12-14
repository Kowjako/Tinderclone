using AutoMapper;
using DatingAppAPI.Application.CQRS.Admin.Requests.Command;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Common;
using MediatR;

namespace DatingAppAPI.Application.CQRS.Admin.Handlers.Command
{
    public class ApprovePhotoHandler : IRequestHandler<ApprovePhotoRequest, PhotoDTO>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public ApprovePhotoHandler(IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<PhotoDTO> Handle(ApprovePhotoRequest request, CancellationToken cancellationToken)
        {
            var photo = await _uow.PhotoRepository.GetPhotoById(request.PhotoId);

            if (photo == null) throw new HttpException(404, "Could not find photo");
            photo.IsApproved = true;

            var user = await _uow.UserRepository.GetUserByPhotoId(request.PhotoId);
            if (!user.Photos.Any(p => p.IsMain)) photo.IsMain = true;

            if (!await _uow.Complete()) throw new HttpException(400, "Fail to change photo status");
            return _mapper.Map<PhotoDTO>(photo);
        }
    }
}
