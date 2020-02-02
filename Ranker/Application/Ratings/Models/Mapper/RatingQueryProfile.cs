using AutoMapper;

namespace Ranker.Application.Ratings.Models.Mapper
{
    public sealed class RatingQueryProfile : Profile
    {
        public RatingQueryProfile()
        {
            CreateMap<MovieRatingQuery, RatingQuery>();
            CreateMap<UserRatingQuery, RatingQuery>();
        }
    }
}
