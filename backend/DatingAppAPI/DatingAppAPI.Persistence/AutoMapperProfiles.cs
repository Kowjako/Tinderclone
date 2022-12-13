using AutoMapper;
using DatingAppAPI.Application.DTO;
using DatingAppAPI.Domain.Entities;
using DatingAppAPI.Persistence.Extensions;

namespace DatingAppAPI.Persistence
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(p => p.PhotoUrl, c => c.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(p => p.Age, c => c.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();

            CreateMap<RegisterDTO, AppUser>();

            CreateMap<Message, MessageDTO>()
                .ForMember(p => p.SenderPhotoUrl, c => c.MapFrom(src => src.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(p => p.ReceiverPhotoUrl, c => c.MapFrom(src => src.Receiver.Photos.FirstOrDefault(p => p.IsMain).Url));


            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc)
                                                                           : null);

            CreateMap<Photo, PhotoForApprovalDTO>()
                .ForMember(p => p.Username, c => c.MapFrom(src => src.AppUser.UserName));
        }
    }
}
