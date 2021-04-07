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
    [Route("v1/users")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public sealed class UserRatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;

        public UserRatingsController(IRatingService ratingService, IMapper mapper)
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
        ///     GET /users/{userId}/ratings?page=5&#x26;size=10
        ///
        /// </remarks>
        [HttpGet("{userId:long}/ratings", Name = nameof(GetUserRatingList))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Tags = new[] { "Users" })]
        public async Task<IActionResult> GetUserRatingList([FromRoute] long userId, [FromQuery] UserRatingQuery query)
        {
            var ratingQuery = _mapper.Map<RatingQuery>(query);
            ratingQuery.UserId = userId;
            var ratings = await _ratingService.GetRatingList(ratingQuery).ConfigureAwait(true);
            Response.AddPaginationHeader(ratings, nameof(GetUserRatingList), query, Url);
            return Ok(new RatingList(ratings));
        }
    }
}
