namespace TripleMatch.Domain.Models.DataBaseModels
{
    public class History : BaseModel
    {
        public int Score { get; set; } = 0;
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        public Guid UserId {  get; set; }
        public virtual User User { get; set; } = null!;
    }
}
