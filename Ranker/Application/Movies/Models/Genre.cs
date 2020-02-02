using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Ranker.Application.Movies.Models
{
    [DataContract(Name = "Genre", Namespace = "")]
    public sealed class Genre
    {
        [DataMember]
        public string Name { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name;
        }

        internal static string NormalizeGenres(IReadOnlyCollection<string> genres)
        {
            var genreList = genres
                ?.ToList()
                .OrderBy(genre => genre)
                .Select(genre => new CultureInfo("en-US", false).TextInfo.ToTitleCase(genre))
                .ToList()
                ?? new List<string>();

            return string.Join("|", genreList);
        }

        internal static IReadOnlyCollection<Genre> ToList(string genres)
        {
            return genres
                .Split("|", StringSplitOptions.RemoveEmptyEntries)
                .Select(genre => new Genre { Name = genre })
                .ToList();
        }

        internal static IReadOnlyCollection<string> ToStringList(string genres)
        {
            return genres
                .Split("|", StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}
