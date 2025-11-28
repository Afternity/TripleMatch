using AutoMapper;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.VMs;
using TripleMatch.Shered.Contracts.VMs.LookupDTOs;

namespace TripleMatch.Application.Common.Mappings
{
    public class HistoryMapping
        : Profile
    {
        public HistoryMapping()
        {
            CreateMap<WriteHistoryDto, History>();

            CreateMap<History, FiveBestHistoriesScoreLookupDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.Rank, opt => opt.Ignore());

            CreateMap<History, UserHistoriesLookupDto>();
            CreateMap<History, UserLastHistoryVm>();
            CreateMap<History, BestUserHistoryVm>();
        }
    }
}
