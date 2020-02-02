using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ranker.Application.Ratings.Models
{
    [CollectionDataContract(Name = "Ratings", Namespace = "")]
    public sealed class RatingList : IEnumerable<RatingDetail>
    {
        private readonly List<RatingDetail> _ratings = new List<RatingDetail>();

        public RatingList()
        {
        }

        public RatingList(IEnumerable<RatingDetail> collection)
        {
            _ratings = new List<RatingDetail>(collection);
        }

        internal void Add(RatingDetail item)
        {
            _ratings.Add(item);
        }

        public IEnumerator<RatingDetail> GetEnumerator() => _ratings.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _ratings.GetEnumerator();
    }
}
