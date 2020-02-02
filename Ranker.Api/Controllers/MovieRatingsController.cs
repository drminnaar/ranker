using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ranker.Application.Ratings;
using Ranker.Application.Ratings.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Ranker.Api.Controllers
{
    [Route("v1/movies")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public sealed class MovieRatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;

        public MovieRatingsController(IRatingService ratingService, IMapper mapper)
        {
            _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get a list of ratings based on specified query parameters
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /movies/{movieId}/ratings?page=5&#x26;size=10
        ///
        /// </remarks>
        [HttpGet("{movieId:long}/ratings", Name = nameof(GetMovieRatingList))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Tags = new[] { "Movies" })]
        public async Task<IActionResult> GetMovieRatingList([FromRoute]long movieId, [FromQuery]MovieRatingQuery query)
        {
            var ratingQuery = _mapper.Map<RatingQuery>(query);
            ratingQuery.MovieId = movieId;
            var ratings = await _ratingService.GetRatingList(ratingQuery);
            Response.AddPaginationHeader(ratings, nameof(GetMovieRatingList), query, Url);
            return Ok(new RatingList(ratings));
        }
    }
}
