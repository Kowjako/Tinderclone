using AutoMapper;
using DatingAppAPI.DTO;
using DatingAppAPI.Persistence.Entities;
using DatingAppAPI.Persistence.Extensions;
using System.Linq;

namespace DatingAppAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(p => p.PhotoUrl, c => c.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(p => p.Age, c => c.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
        }
    }
}
