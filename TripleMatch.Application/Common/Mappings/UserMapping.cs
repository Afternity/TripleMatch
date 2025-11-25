using AutoMapper;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.Application.Common.Mappings
{
    public class UserMapping 
        : Profile
    {
        public UserMapping()
        {
            CreateMap<AuthDto, User>();
            CreateMap<RegistrationDto, User>();
            CreateMap<UpdateProfileDto, User>();

            CreateMap<User, UserProfileVm>();
        }
    }
}
