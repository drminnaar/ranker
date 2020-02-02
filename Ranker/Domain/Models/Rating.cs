using System;
using System.Collections.Generic;

namespace Ranker.Domain.Models
{
    public sealed class Rating
    {
        public long RatingId { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
        public long MovieId { get; set; }
        public Movie? Movie { get; set; }
        public double Score { get; set; }
        public long Timestamp { get; set; }

        public static IReadOnlyCollection<Rating> ToReadOnlyCollection() => Array.Empty<Rating>();
    }
}
