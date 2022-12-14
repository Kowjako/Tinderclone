using DatingAppAPI.Application.CQRS.UserLike.Requests.Command;
using DatingAppAPI.Application.Errors;
using DatingAppAPI.Application.Interfaces.Common;
using MediatR;
using UL = DatingAppAPI.Domain.Entities.UserLike;

namespace DatingAppAPI.Application.CQRS.UserLike.Handlers.Command
{
    public class AddLikeHandler : IRequestHandler<AddLikeRequest, Unit>
    {
        private readonly IUnitOfWork _uow;

        public AddLikeHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Unit> Handle(AddLikeRequest request, CancellationToken cancellationToken)
        {
            var likedUser = await _uow.UserRepository.GetUserByUsernameAsync(request.Username);
            var sourceUser = await _uow.LikesRepository.GetUserWithLikes(request.SourceUserId);

            if (likedUser == null) throw new HttpException(404, string.Empty);
            if (sourceUser.UserName.Equals(request.Username)) 
                throw new HttpException(400, "You cannot like yourself");

            var userLike = await _uow.LikesRepository.GetUserLike(request.SourceUserId, likedUser.Id);
            if (userLike != null) throw new HttpException(400, "You already like this user");

            userLike = new UL()
            {
                SourceUserId = request.SourceUserId,
                TargetUserId = likedUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);
            if (!await _uow.Complete())
                throw new HttpException(400, "Failed to like user");
            return Unit.Value;
        }
    }
}
