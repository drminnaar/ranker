using System.Runtime.Serialization;

namespace Ranker.Application.Users.Models
{
    [DataContract(Name = "User", Namespace = "")]
    public sealed class UserDetail
    {
        [DataMember(Order = 1)]
        public long UserId { get; set; }

        [DataMember(Order = 2)]
        public long Age { get; set; }

        [DataMember(Order = 3)]
        public string FirstName { get; set; } = string.Empty;

        [DataMember(Order = 4)]
        public string LastName { get; set; } = string.Empty;

        [DataMember(Order = 5)]
        public string Gender { get; set; } = string.Empty;

        [DataMember(Order = 6)]
        public string Email { get; set; } = string.Empty;
    }
}
