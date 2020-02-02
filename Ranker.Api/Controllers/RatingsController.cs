using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ranker.Application.Ratings;
using Ranker.Application.Ratings.Models;

namespace Ranker.Api.Controllers
{
    [Route("v1/ratings")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public sealed class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
        }

        /// <summary>
        /// Create a new rating
        /// </summary>
        /// <response code="201">The rating was created successfully. Also includes 'location' header to newly created rating</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPost(Name = nameof(CreateRating))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateRating([FromBody]RatingForCreate rating)
        {
            var createdRating = await _ratingService.CreateRating(rating);

            return CreatedAtAction(
                actionName: nameof(GetRatingById),
                routeValues: new { ratingId = createdRating.RatingId },
                value: createdRating);
        }

        /// <summary>
        /// Delete rating
        /// </summary>
        /// <response code="204">The rating was deleted successfully.</response>
        /// <response code="404">A rating having specified rating id was not found</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpDelete("{ratingId:long}", Name = nameof(DeleteRating))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRating([FromRoute]long ratingId)
        {
            await _ratingService.DeleteRating(ratingId);
            return NoContent();
        }

        /// <summary>
        /// Returns metadata in the header of the response that describes what other methods
        /// and operations are supported at this URL
        /// </summary>
        /// <returns>Supported methods in header of response</returns>
        [HttpOptions(Name = nameof(GetRatingOptions))]
        public IActionResult GetRatingOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,PATCH,POST,PUT,DELETE");

            return Ok();
        }

        /// <summary>
        /// Get a single rating by unique rating id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /ratings/123
        ///
        /// </remarks>
        [HttpGet("{ratingId:long}", Name = nameof(GetRatingById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<IActionResult> GetRatingById(long ratingId)
        {
            var rating = await _ratingService.GetRating(ratingId);

            if (rating == null)
                return NotFound();

            return Ok(rating);
        }

        /// <summary>
        /// Get a list of ratings based on specified query parameters
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /ratings?userid=123&#x26;page=5&#x26;size=10
        ///
        /// </remarks>
        [HttpGet(Name = nameof(GetRatingList))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<IActionResult> GetRatingList([FromQuery]RatingQuery query)
        {
            var ratings = await _ratingService.GetRatingList(query);
            Response.AddPaginationHeader(ratings, nameof(GetRatingList), query, Url);
            return Ok(new RatingList(ratings));
        }

        /// <summary>
        /// Update an existing rating
        /// </summary>
        /// <response code="204">The rating was updated successfully</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The rating was not found for specified rating id</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPut("{ratingId:long}", Name = nameof(UpdateScore))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateScore([FromRoute]long ratingId, [FromBody]ScoreUpdate score)
        {
            await _ratingService.UpdateRatingScore(ratingId, score);
            return NoContent();
        }
    }
}
