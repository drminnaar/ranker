using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ranker.Application.Movies.Filters;
using Ranker.Application.Movies.Models;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Movies
{
    public sealed class MovieService : IMovieService
    {
        private readonly IRatingsDbContext _context;
        private readonly IMovieFilterBuilder _filter;
        private readonly IEntityOrderBuilder<Movie> _order;
        private readonly IMapper _mapper;

        public MovieService(
            IRatingsDbContext context,
            IMovieFilterBuilder filter,
            IEntityOrderBuilder<Movie> order,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _order = order ?? throw new ArgumentNullException(nameof(order));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<MovieDetail> CreateMovie(MovieForCreate movieForCreate)
        {
            if (movieForCreate is null)
                throw new ArgumentNullException(nameof(movieForCreate));

            return CreateMovie();

            async Task<MovieDetail> CreateMovie()
            {
                var movie = _mapper.Map<Movie>(movieForCreate);
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return await GetMovie(movie.MovieId);
            }
        }

        public async Task DeleteMovie(long movieId)
        {
            var movieFromDb = await _context
                .Movies
                .FirstOrDefaultAsync(movie => movie.MovieId == movieId);

            if (movieFromDb == null)
                throw new EntityNotFoundException($"A movie having id '{movieId}' could not be found");

            _context.Movies.Remove(movieFromDb);
            await _context.SaveChangesAsync();
        }

        public async Task<MovieDetail> GetMovie(long movieId)
        {
            var movieFromDb = await _context
                .Movies
                .FirstOrDefaultAsync(movie => movie.MovieId == movieId);

            return _mapper.Map<MovieDetail>(movieFromDb);
        }

        public async Task<MovieForPatch?> GetMovieForPatch(long movieId)
        {
            var movieFromDb = await _context
                .Movies
                .TagWithQueryName(nameof(GetMovieForPatch))
                .AsNoTracking()
                .FirstOrDefaultAsync(movie => movie.MovieId == movieId);

            return movieFromDb == null ? null : _mapper.Map<MovieForPatch>(movieFromDb);
        }

        public Task<IPagedCollection<MovieDetail>> GetMovies(MovieQuery query)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            return GetMovies();

            async Task<IPagedCollection<MovieDetail>> GetMovies()
            {
                var filter = _filter
                    .WhereGenresEquals(query.Genres)
                    .WhereTitleEquals(query.Title)
                    .Filter;

                var moviesFromDb = await _context
                    .Movies
                    .Where(filter)
                    .OrderBy(query.Order, _order)
                    .ToPagedCollectionAsync(query.Page, query.Limit);

                var movies = _mapper.Map<IReadOnlyList<MovieDetail>>(moviesFromDb);

                return new PagedCollection<MovieDetail>(
                    movies,
                    moviesFromDb.ItemCount,
                    moviesFromDb.CurrentPageNumber,
                    moviesFromDb.PageSize);
            }
        }

        public Task PatchMovie(long movieId, MovieForPatch movieForPatch)
        {
            if (movieForPatch is null)
                throw new ArgumentNullException(nameof(movieForPatch));

            return PatchMovie();

            async Task PatchMovie()
            {
                var movieFromDb = await _context
                    .Movies
                    .FirstOrDefaultAsync(movie => movie.MovieId == movieId);

                if (movieFromDb == null)
                    throw new EntityNotFoundException($"A movie having id '{movieId}' could not be found");

                movieFromDb.Title = movieForPatch.Title;
                movieFromDb.Genres = Genre.NormalizeGenres(movieForPatch.Genres);

                await _context.SaveChangesAsync();
            }
        }

        public Task UpdateMovie(long movieId, MovieForUpdate movieForUpdate)
        {
            if (movieForUpdate is null)
                throw new ArgumentNullException(nameof(movieForUpdate));

            return UpdateMovie();

            async Task UpdateMovie()
            {
                var movieFromDb = await _context
                    .Movies
                    .FirstOrDefaultAsync(movie => movie.MovieId == movieId);

                if (movieFromDb == null)
                    throw new EntityNotFoundException($"A movie having id '{movieId}' could not be found");

                movieFromDb.Title = movieForUpdate.Title;
                movieFromDb.Genres = Genre.NormalizeGenres(movieForUpdate.Genres);

                await _context.SaveChangesAsync();
            }
        }
    }
}
