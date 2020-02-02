using System;
using System.Collections.Generic;

namespace Ranker.Domain.Models
{
    public sealed class MovieTag
    {
        public long TagId { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
        public long MovieId { get; set; }
        public Movie? Movie { get; set; }
        public string Tag { get; set; } = string.Empty;
        public long Timestamp { get; set; }

        public static IReadOnlyCollection<MovieTag> ToReadOnlyCollection() => Array.Empty<MovieTag>();
    }
}
