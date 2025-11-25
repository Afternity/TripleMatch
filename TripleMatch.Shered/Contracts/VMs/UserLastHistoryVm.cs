namespace TripleMatch.Shered.Contracts.VMs
{
    public class UserLastHistoryVm
    {
        public int Score { get; set; } = 0;
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
