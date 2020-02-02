using System.Collections.Generic;
using AutoMapper;
using Ranker.Domain.Models;

namespace Ranker.Application.Ratings.Models
{
    public sealed class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<RatingForCreate, Rating>();
            CreateMap<Rating, RatingDetail>();
            CreateMap<IPagedCollection<Rating>, List<RatingDetail>>();
        }
    }
}
