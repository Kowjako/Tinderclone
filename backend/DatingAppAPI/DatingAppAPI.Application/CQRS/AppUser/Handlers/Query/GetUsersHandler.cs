using DatingAppAPI.Application.CQRS.AppUser.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Pagination;
using MediatR;

namespace DatingAppAPI.Application.CQRS.AppUser.Handlers.Query
{
    public class GetUsersHandler : IRequestHandler<GetUsersRequest, PagedList<MemberDTO>>
    {
        private readonly IUnitOfWork _uow;

        public GetUsersHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedList<MemberDTO>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var gender = await _uow.UserRepository.GetUserGender(request.Username);
            request.Params.CurrentUsername = request.Username;

            if (string.IsNullOrEmpty(request.Params.Gender))
            {
                request.Params.Gender = gender == "male" ? "female" : "male";
            }

            var users = await _uow.UserRepository.GetMembersAsync(request.Params);
            return users;
        }
    }
}
