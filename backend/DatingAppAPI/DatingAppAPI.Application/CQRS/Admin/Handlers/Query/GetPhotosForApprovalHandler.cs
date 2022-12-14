using DatingAppAPI.Application.CQRS.Admin.Requests.Query;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Application.Interfaces.Common;
using MediatR;

namespace DatingAppAPI.Application.CQRS.Admin.Handlers.Query
{
    public class GetPhotosForApprovalHandler : IRequestHandler<GetPhotosForApprovalRequest, IEnumerable<PhotoForApprovalDTO>>
    {
        private readonly IUnitOfWork _uow;

        public GetPhotosForApprovalHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<PhotoForApprovalDTO>> Handle(GetPhotosForApprovalRequest request, CancellationToken cancellationToken)
        {
            return await _uow.PhotoRepository.GetUnapprovedPhotos();
        }
    }
}
