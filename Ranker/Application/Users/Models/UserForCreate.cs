namespace Ranker.Application.Users.Models
{
    public sealed class UserForCreate
    {
        public long Age { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
