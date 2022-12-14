using DatingAppAPI.Application.CQRS.Message.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Common;
using DatingAppAPI.Application.Interfaces.Pagination;
using MediatR;

namespace DatingAppAPI.Application.CQRS.Message.Handlers.Query
{
    public class GetMessagesForUserHandler : IRequestHandler<GetMessagesForUserRequest, PagedList<MessageDTO>>
    {
        private readonly IUnitOfWork _uow;

        public GetMessagesForUserHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedList<MessageDTO>> Handle(GetMessagesForUserRequest request, CancellationToken cancellationToken)
        {
            return await _uow.MessageRepository.GetMessagesForUser(request.Params);
        }
    }
}
