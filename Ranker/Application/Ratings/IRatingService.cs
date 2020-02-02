using System.Collections.Generic;
using System.Threading.Tasks;
using Ranker.Application.Ratings.Models;

namespace Ranker.Application.Ratings
{
    public interface IRatingService
    {
        Task<RatingDetail> CreateRating(RatingForCreate rating);
        Task DeleteRating(long ratingId);
        Task<RatingDetail?> GetRating(long ratingId);
        Task<RatingDetail?> GetRating(long movieId, long userId);
        Task<IPagedCollection<RatingDetail>> GetRatingList(RatingQuery query);
        Task UpdateRatingScore(long ratingId, ScoreUpdate score);
    }
}
