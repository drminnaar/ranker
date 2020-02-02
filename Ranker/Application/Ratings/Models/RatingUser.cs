using System.Runtime.Serialization;

namespace Ranker.Application.Ratings.Models
{
    [DataContract(Name = "User", Namespace = "")]
    public sealed class RatingUser
    {
        [DataMember(Order = 1)]
        public long UserId { get; set; }

        [DataMember(Order = 1)]
        public long Age { get; set; }

        [DataMember(Order = 1)]
        public string FirstName { get; set; } = string.Empty;

        [DataMember(Order = 1)]
        public string LastName { get; set; }= string.Empty;

        [DataMember(Order = 1)]
        public string Gender { get; set; }= string.Empty;

        [DataMember(Order = 1)]
        public string Email { get; set; }= string.Empty;

        internal static RatingUser Default() => new RatingUser();
    }
}
