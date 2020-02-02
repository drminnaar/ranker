using System;
using System.Collections.Generic;

namespace Ranker.Domain.Models
{
    public class Movie
    {
        public long MovieId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Genres { get; set; } = string.Empty;
        public virtual ICollection<Rating> Ratings { get; } = new HashSet<Rating>();

        public static Movie Default() => new Movie();
        public static IReadOnlyCollection<Movie> ToReadOnlyCollection() => Array.Empty<Movie>();
    }
}
