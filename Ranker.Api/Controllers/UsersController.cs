using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Ranker.Api.Models.Users;
using Ranker.Application.Users;
using Ranker.Application.Users.Models;

namespace Ranker.Api.Controllers
{
    [Route("v1/users")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    public sealed class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IETagProvider _etag;
        private readonly UserResourceFactory _resourceFactory;

        public UsersController(IUserService userService, IETagProvider etag, UserResourceFactory resourceFactory)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _etag = etag ?? throw new ArgumentNullException(nameof(etag));
            _resourceFactory = resourceFactory ?? throw new ArgumentNullException(nameof(resourceFactory));
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <response code="201">The user was created successfully. Also includes 'location' header to newly created user</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPost(Name = nameof(CreateUser))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateUser([FromBody] UserForCreate user)
        {
            var createdUser = await _userService.CreateUser(user).ConfigureAwait(true);

            return CreatedAtAction(
                actionName: nameof(GetUserById),
                routeValues: new { userId = createdUser.UserId },
                value: _resourceFactory.CreateUserResource(createdUser));
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <response code="204">The user was deleted successfully.</response>
        /// <response code="404">A user having specified user id was not found</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpDelete("{userId:long}", Name = nameof(DeleteUser))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser([FromRoute] long userId)
        {
            await _userService.DeleteUser(userId).ConfigureAwait(true);
            return NoContent();
        }

        /// <summary>
        /// Returns metadata in the header of the response that describes what other methods
        /// and operations are supported at this URL
        /// </summary>
        /// <returns>Supported methods in header of response</returns>
        [HttpOptions(Name = nameof(GetUserOptions))]
        public IActionResult GetUserOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,PATCH,POST,PUT,DELETE");

            return Ok();
        }

        /// <summary>
        /// Get a single user by user id
        /// </summary>
        /// <response code="200">The user was found</response>
        /// <response code="404">The user was not found</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet("{userId:long}", Name = nameof(GetUserById))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResource>> GetUserById([FromRoute] long userId)
        {
            var user = await _userService.GetUser(userId).ConfigureAwait(true);

            if (user == null)
                return NotFound();

            var responseETag = _etag.GetETag(user);
            if (Request.HasETagHeader() && responseETag == Request.GetETagHeader())
            {
                Response.Headers[HeaderNames.ContentLength] = "0";
                Response.ContentType = string.Empty;
                return StatusCode((int)HttpStatusCode.NotModified);
            }
            Response.AddETagHeader(responseETag);

            // To return only a user excluding links, use the following return
            //return Ok(user);

            return Ok(_resourceFactory.CreateUserResource(user));
        }

        /// <summary>
        /// Get a paginated list of users. The pagination metadata is contained within 'x-pagination' header of response.
        /// </summary>
        /// <response code="200">A paginated list of users</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpGet(Name = nameof(GetUserList))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ResponseCache(CacheProfileName = "Default10")]
        public async Task<IActionResult> GetUserList([FromQuery] UserQuery query)
        {
            var users = await _userService.GetUserList(query).ConfigureAwait(true);
            Response.AddPaginationHeader(users, nameof(GetUserList), query, Url);

            // To return only a user list excluding links, use the following return
            //return Ok(new UserList(users));

            return Ok(_resourceFactory.CreateUserResourceList(users, query));
        }

        /// <summary>
        /// Do partial user update
        /// </summary>
        /// <response code="204">The user was updated successfully</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The user was not found for specified user id</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPatch("{userId:long}", Name = nameof(PatchUser))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchUser([FromRoute] long userId, [FromBody] JsonPatchDocument<UserForPatch> patch)
        {
            var userForPatch = await _userService.GetUserForPatch(userId).ConfigureAwait(true);

            if (userForPatch == null)
                return NotFound();

            patch.ApplyTo(userForPatch, ModelState);

            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            await _userService.PatchUser(userId, userForPatch).ConfigureAwait(true);
            return NoContent();
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <response code="204">The user was updated successfully</response>
        /// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
        /// <response code="404">The user was not found for specified user id</response>
        /// <response code="406">When a request is specified in an unsupported content type using the Accept header</response>
        /// <response code="415">When a response is specified in an unsupported content type</response>
        /// <response code="422">If query params structure is valid, but the values fail validation</response>
        /// <response code="500">A server fault occurred</response>
        [HttpPut("{userId:long}", Name = nameof(UpdateUser))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromRoute] long userId, [FromBody] UserForUpdate user)
        {
            await _userService.UpdateUser(userId, user).ConfigureAwait(true);
            return NoContent();
        }
    }
}
