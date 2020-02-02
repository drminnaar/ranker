using System.Runtime.Serialization;

namespace Ranker.Application.Ratings.Models
{
    [DataContract(Name = "Rating", Namespace = "")]
    public sealed class RatingDetail
    {
        [DataMember(Order = 1)]
        public long RatingId { get; set; }

        [DataMember(Order = 2)]
        public double Score { get; set; }

        [DataMember(Order = 3)]
        public long Timestamp { get; set; }

        [DataMember(Order = 4)]
        public RatingUser User { get; set; } = RatingUser.Default();

        [DataMember(Order = 5)]
        public RatingMovie Movie { get; set; } = RatingMovie.Default();
    }
}
