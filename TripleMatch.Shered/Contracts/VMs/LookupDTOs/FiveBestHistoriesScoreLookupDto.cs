namespace TripleMatch.Shered.Contracts.VMs.LookupDTOs
{
    public class FiveBestHistoriesScoreLookupDto
    {
        public int Score { get; set; } = 0;
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string FullName { get; set; } = string.Empty;
    }
}
