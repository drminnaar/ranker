using AutoMapper;
using Ranker.Domain.Models;

namespace Ranker.Application.Ratings.Models.Mapper
{
    public sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, RatingUser>();
        }
    }
}
