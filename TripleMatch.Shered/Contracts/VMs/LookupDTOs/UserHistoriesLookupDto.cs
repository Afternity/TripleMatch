namespace TripleMatch.Shered.Contracts.VMs.LookupDTOs
{
    public class UserHistoriesLookupDto
    {
        public int Score { get; set; } = 0;
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
    }
}
