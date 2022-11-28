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
        }
    }
}
