using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Pagination;
using MediatR;

namespace DatingAppAPI.Application.CQRS.Message.Requests.Query
{
    public class GetMessagesForUserRequest : IRequest<PagedList<MessageDTO>>
    {
        public MessageParams Params { get; set; }
    }
}
