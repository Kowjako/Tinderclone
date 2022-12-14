using AutoMapper;
using DatingAppAPI.Application.CQRS.AppUser.Requests.Command;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Common;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Handlers.Command
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, Unit>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public UpdateUserHandler(IMapper mapper, IUnitOfWork uow)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            var user = await _uow.UserRepository.GetUserByUsernameAsync(request.Username);

            if (user == null) throw new HttpException(404, string.Empty);

            _mapper.Map(request.MemberUpdateDTO, user);

            if (!await _uow.Complete()) throw new HttpException(400, "Failed to update user");
            return Unit.Value;
        }
    }
}
