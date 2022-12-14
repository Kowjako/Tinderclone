using DatingAppAPI.Application.CQRS.AppUser.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Common;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Handlers.Query
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, MemberDTO>
    {
        private readonly IUnitOfWork _uow;

        public GetUserByIdHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<MemberDTO> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
        {
            return await _uow.UserRepository.GetMemberAsync(request.Username, request.SelfRequest);
        }
    }
}
