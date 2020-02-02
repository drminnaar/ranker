using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ranker.Application.Movies.Models
{
    [DataContract(Name = "Movie", Namespace = "")]
    public sealed class MovieDetail
    {
        [DataMember(Order = 1)]
        public long MovieId { get; set; }

        [DataMember(Order = 2)]
        public string Title { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public IEnumerable<Genre> Genres { get; set; } = new List<Genre>();
    }
}
