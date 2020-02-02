using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ranker.Application.Movies.Models
{
    public sealed class MovieForUpdate
    {
        public string Title { get; set; } = string.Empty;
        public IReadOnlyCollection<string> Genres { get; set; } = new List<string>();
    }
}
