using Microsoft.AspNetCore.Mvc;

namespace Ranker.Application.Ratings.Models
{
    public sealed class RatingQuery : PagedQueryParams
    {
        private const string DEFAULT_ORDER = "-timestamp"; // show most recent ratings first

        [FromQuery(Name = "userid")]
        public long? UserId { get; set; }

        [FromQuery(Name = "movieid")]
        public long? MovieId { get; set; }

        [FromQuery(Name ="min-score")]
        public string? MinimumScore { get; set; }

        [FromQuery(Name = "max-score")]
        public string? MaximumScore { get; set; }

        [FromQuery(Name = "min-timestamp")]
        public string? MinimumTimestamp { get; set; }

        [FromQuery(Name = "max-timestamp")]
        public string? MaximumTimestamp { get; set; }

        [FromQuery(Name = "order")]
        public string Order { get; set; } = DEFAULT_ORDER;
    }
}
