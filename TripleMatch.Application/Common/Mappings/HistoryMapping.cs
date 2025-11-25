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
            CreateMap<CreateHistoryDto, History>();

            CreateMap<History, FiveBestHistoriesScoreLookupDto>();
            CreateMap<History, UserHistoriesLookupDto>();
            CreateMap<History, UserLastHistoryVm>();
        }
    }
}
