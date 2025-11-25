namespace TripleMatch.Shered.Contracts.DTOs
{
    public class CreateHistoryDto
    {
        public int Score { get; set; } = 0;

        public Guid UserId { get; set; }
    }
}
