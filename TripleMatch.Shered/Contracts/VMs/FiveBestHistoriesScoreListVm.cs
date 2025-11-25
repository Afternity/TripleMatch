using TripleMatch.Shered.Contracts.VMs.LookupDTOs;

namespace TripleMatch.Shered.Contracts.VMs
{
    public class FiveBestHistoriesScoreListVm
    {
        public virtual ICollection<FiveBestHistoriesScoreLookupDto> Histories { get; set; } = [];
    }
}
