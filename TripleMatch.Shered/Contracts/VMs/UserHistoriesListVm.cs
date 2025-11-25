using TripleMatch.Shered.Contracts.VMs.LookupDTOs;

namespace TripleMatch.Shered.Contracts.VMs
{
    public class UserHistoriesListVm
    {
        public virtual ICollection<UserHistoriesLookupDto> Histories { get; set; } = [];
    }
}
