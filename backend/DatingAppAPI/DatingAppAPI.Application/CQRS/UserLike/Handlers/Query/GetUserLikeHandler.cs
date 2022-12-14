using DatingAppAPI.Application.CQRS.UserLike.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Pagination;
using MediatR;

namespace DatingAppAPI.Application.CQRS.UserLike.Handlers.Query
{
    public class GetUserLikeHandler : IRequestHandler<GetUserLikesRequest, PagedList<LikeDTO>>
    {
        private readonly IUnitOfWork _uow;

        public GetUserLikeHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedList<LikeDTO>> Handle(GetUserLikesRequest request, CancellationToken cancellationToken)
        {
            request.Params.UserId = int.Parse(request.UserId);

            return await _uow.LikesRepository.GetUserLikes(request.Params);
        }
    }
}
