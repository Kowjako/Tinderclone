using AutoMapper;
using DatingAppAPI.Application.CQRS.AppUser.Requests.Command;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Services;
using DatingAppAPI.Domain.Entities;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Handlers.Command
{
    public class AddPhotoHandler : IRequestHandler<AddPhotoRequest, PhotoDTO>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public AddPhotoHandler(IPhotoService photoService, IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _photoService = photoService;
            _mapper = mapper;
        }

        public async Task<PhotoDTO> Handle(AddPhotoRequest request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(request.Username);

            if (user == null) throw new HttpException(404, string.Empty);

            var result = await _photoService.AddPhotoAsync(request.File);

            if (result.Error != null) throw new HttpException(400, result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            user.Photos.Add(photo);

            if(await _uow.Complete())
            {
                return _mapper.Map<PhotoDTO>(photo);
            }

            return null;
        }
    }
}
