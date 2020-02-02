using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ranker.Application.Movies.Models
{
    [CollectionDataContract(Name = "Movies", Namespace = "")]
    public sealed class MovieList : IEnumerable<MovieDetail>
    {
        private readonly List<MovieDetail> _movies = new List<MovieDetail>();

        public MovieList()
        {
        }

        public MovieList(IEnumerable<MovieDetail> collection)
        {
            _movies = new List<MovieDetail>(collection);
        }

        internal void Add(MovieDetail item)
        {
            _movies.Add(item);
        }

        public IEnumerator<MovieDetail> GetEnumerator() => _movies.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _movies.GetEnumerator();
    }
}
