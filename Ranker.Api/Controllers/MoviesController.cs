using System;
using System.Threading.Tasks;
using Ranker.Application.Movies;
using Ranker.Application.Movies.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace Ranker.Api.Controllers
{
    [Route("v1/movies")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
        }

        /// <summary>
        /// Create a new movie
        /// </summary>
        /// <response code="201">The movie was created successfully. Also includes 'location' header to newly created movie</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPost(Name = nameof(CreateMovie))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMovie([FromBody]MovieForCreate movie)
        {
            var createdMovie = await _movieService.CreateMovie(movie);

            return CreatedAtAction(
                actionName: nameof(GetMovieById),
                routeValues: new { movieId = createdMovie.MovieId },
                value: createdMovie);
        }

        /// <summary>
        /// Delete movie
        /// </summary>
        /// <response code="204">The movie was deleted successfully.</response>
        /// <response code="404">A movie having specified movie id was not found</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpDelete("{movieId:long}", Name = nameof(DeleteMovie))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMovie([FromRoute]long movieId)
        {
            await _movieService.DeleteMovie(movieId);
            return NoContent();
        }

        /// <summary>
        /// Returns metadata in the header of the response that describes what other methods
        /// and operations are supported at this URL
        /// </summary>
        /// <returns>Supported methods in header of response</returns>
        [HttpOptions(Name = nameof(GetMovieOptions))]
        public IActionResult GetMovieOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,PATCH,POST,PUT,DELETE");

            return Ok();
        }

        /// <summary>
        /// Get a single movie by movie id
        /// </summary>
        /// <response code="200">The movie was found</response>
        /// <response code="404">The movie was not found</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet("{movieId:long}", Name = nameof(GetMovieById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<IActionResult> GetMovieById([FromRoute]long movieId)
        {
            var movie = await _movieService.GetMovie(movieId);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }

        /// <summary>
        /// Get a paginated list of movies. The pagination metadata is contained within 'x-pagination' header of response.
        /// </summary>
        /// <response code="200">A paginated list of movies</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet(Name = nameof(GetMovieList))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<IActionResult> GetMovieList([FromQuery]MovieQuery query)
        {
            var movies = await _movieService.GetMovies(query);
            Response.AddPaginationHeader(movies, nameof(GetMovieList), query, Url);
            return Ok(new MovieList(movies));
        }

        /// <summary>
        /// Do partial movie update
        /// </summary>
        /// <response code="204">The movie was updated successfully</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The movie was not found for specified movie id</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPatch("{movieId:long}", Name = nameof(PatchMovie))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchMovie([FromRoute]long movieId, [FromBody]JsonPatchDocument<MovieForPatch> patch)
        {
            var movieForPatch = await _movieService.GetMovieForPatch(movieId);

            if (movieForPatch == null)
                return NotFound();

            patch.ApplyTo(movieForPatch, ModelState);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            await _movieService.PatchMovie(movieId, movieForPatch);
            return NoContent();
        }

        /// <summary>
        /// Update an existing movie
        /// </summary>
        /// <response code="204">The movie was updated successfully</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The movie was not found for specified movie id</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPut("{movieId:long}", Name = nameof(UpdateMovie))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMovie([FromRoute]long movieId, [FromBody]MovieForUpdate movie)
        {
            await _movieService.UpdateMovie(movieId, movie);
            return NoContent();
        }
    }
}
