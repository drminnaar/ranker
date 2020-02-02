using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ranker.Application.Ratings.Filters;
using Ranker.Application.Ratings.Models;
using Ranker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ranker.Application.Ratings
{
    public sealed class RatingService : IRatingService
    {
        private readonly IRatingsDbContext _context;
        private readonly IRatingsFilterBuilder _filter;
        private readonly IEntityOrderBuilder<Rating> _order;
        private readonly IMapper _mapper;

        public RatingService(
            IRatingsDbContext context,
            IMapper mapper,
            IRatingsFilterBuilder filter,
            IEntityOrderBuilder<Rating> order)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _order = order ?? throw new ArgumentNullException(nameof(order));
        }

        public Task<RatingDetail> CreateRating(RatingForCreate rating)
        {
            if (rating is null)
                throw new ArgumentNullException(nameof(rating));

            return CreateRating();

            async Task<RatingDetail> CreateRating()
            {
                var ratingForCreate = _mapper.Map<Rating>(rating);
                ratingForCreate.Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                _context.Ratings.Add(ratingForCreate);
                await _context.SaveChangesAsync();
                return await GetRating(ratingForCreate.RatingId)
                    ?? throw new InvalidOperationException("Expected a rating from create");
            }
        }

        public async Task DeleteRating(long ratingId)
        {
            var ratingFromDb = await _context
                .Ratings
                .Where(rating => rating.RatingId == ratingId)
                .Include(rating => rating.User)
                .Include(rating => rating.Movie)
                .FirstOrDefaultAsync();

            if (ratingFromDb == null)
                throw new EntityNotFoundException($"A rating having id '{ratingId}' could not be found");

            _context.Ratings.Remove(ratingFromDb);
            await _context.SaveChangesAsync();
        }

        public async Task<RatingDetail?> GetRating(long ratingId)
        {
            var rating = await _context
                .Ratings
                .TagWithQueryName(nameof(GetRating))
                .AsNoTracking()
                .Where(rating => rating.RatingId == ratingId)
                .Include(rating => rating.User)
                .Include(rating => rating.Movie)
                .FirstOrDefaultAsync();

            return rating == null ? null : _mapper.Map<RatingDetail>(rating);
        }

        public async Task<RatingDetail?> GetRating(long movieId, long userId)
        {
            var rating = await _context
                .Ratings
                .TagWithQueryName(nameof(GetRating))
                .AsNoTracking()
                .Where(rating => rating.MovieId == movieId && rating.UserId == userId)
                .Include(rating => rating.User)
                .Include(rating => rating.Movie)
                .FirstOrDefaultAsync();

            return rating == null ? null : _mapper.Map<RatingDetail>(rating);
        }

        public Task<IPagedCollection<RatingDetail>> GetRatingList(RatingQuery query)
        {
            if (query is null)
                throw new ArgumentNullException(nameof(query));

            return GetRatingList();

            async Task<IPagedCollection<RatingDetail>> GetRatingList()
            {
                var filter = _filter
                    .WhereMovieId(query.MovieId)
                    .WhereRating(query.MinimumScore, query.MaximumScore)
                    .WhereTimestamp(query.MinimumTimestamp, query.MaximumTimestamp)
                    .WhereUserId(query.UserId)
                    .Filter;

                var ratingsFromDb = await _context
                    .Ratings
                    .TagWithQueryName(nameof(GetRatingList))
                    .AsNoTracking()
                    .Where(filter)
                    .Include(rating => rating.User)
                    .Include(rating => rating.Movie)
                    .OrderBy(query.Order, _order)
                    .ToPagedCollectionAsync(query.Page, query.Limit);

                var ratings = _mapper.Map<IReadOnlyList<RatingDetail>>(ratingsFromDb);

                return new PagedCollection<RatingDetail>(
                    ratings,
                    ratingsFromDb.ItemCount,
                    ratingsFromDb.CurrentPageNumber,
                    ratingsFromDb.PageSize);
            }
        }

        public Task UpdateRatingScore(long ratingId, ScoreUpdate score)
        {
            if (score is null)
                throw new ArgumentNullException(nameof(score));

            return UpdateRatingScore();

            async Task UpdateRatingScore()
            {
                var ratingFromDb = await _context
                    .Ratings
                    .Where(rating => rating.RatingId == ratingId)
                    .Include(rating => rating.User)
                    .Include(rating => rating.Movie)
                    .FirstOrDefaultAsync();

                if (ratingFromDb == null)
                    throw new EntityNotFoundException($"A rating having id '{ratingId}' could not be found");

                ratingFromDb.Score = score.Score;
                await _context.SaveChangesAsync();
            }
        }
    }
}
