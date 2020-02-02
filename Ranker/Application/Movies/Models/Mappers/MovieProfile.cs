using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Ranker.Domain.Models;

namespace Ranker.Application.Movies.Models.Mappers
{
    public sealed class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieDetail>()
                .ForMember(destination =>
                    destination.Genres,
                    options => options.MapFrom(source => Genre.ToList(source.Genres)));

            CreateMap<IPagedCollection<Movie>, List<MovieDetail>>();

            CreateMap<Movie, MovieForPatch>()
                .ForMember(destination =>
                    destination.Genres,
                    options => options.MapFrom(source => Genre.ToStringList(source.Genres)));

            CreateMap<MovieForCreate, Movie>()
                .ForMember(destination =>
                    destination.Genres,
                    options => options.MapFrom(source => Genre.NormalizeGenres(source.Genres)));

            CreateMap<MovieForPatch, Movie>()
                .ForMember(destination =>
                    destination.Genres,
                    options => options.MapFrom(source => Genre.NormalizeGenres(source.Genres)));
        }
    }
}
