namespace TripleMatch.Domain.Models.DataBaseModels
{
    public class User : BaseModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public virtual ICollection<History> Histories { get; set; } = [];
    }
}
