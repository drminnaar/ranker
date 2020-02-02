using System;
using System.Linq;
using AutoMapper;
using Ranker.Domain.Models;

namespace Ranker.Application.Ratings.Models.Mapper
{
    public sealed class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, RatingMovie>()
                .ForMember(destination =>
                    destination.Genres,
                    options => options.MapFrom(source => source
                        .Genres
                        .Split("|", StringSplitOptions.RemoveEmptyEntries)
                        .Select(genre => new Genre { Name = genre })));
        }
    }
}
