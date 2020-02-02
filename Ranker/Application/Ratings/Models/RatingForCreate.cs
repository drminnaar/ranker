namespace Ranker.Application.Ratings.Models
{
    public sealed class RatingForCreate
    {
        public long UserId { get; set; }
        public long MovieId { get; set; }
        public double Score { get; set; }
    }
}
