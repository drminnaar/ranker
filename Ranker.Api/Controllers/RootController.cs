using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ranker.Api.Controllers
{
    [Route("v1")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    [Consumes("application/json", "application/xml")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public sealed class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(Get))]
        public IActionResult Get()
        {
            var links = new ResourceLinkCollection
            {
                new ResourceLink(Url.Link(nameof(RootController.Get), new { }) ?? string.Empty, "self", HttpMethod.Get.Method),
                new ResourceLink(Url.Link(nameof(MoviesController.GetMovieList), new { }) ?? string.Empty, "movies", HttpMethod.Get.Method),
                new ResourceLink(Url.Link(nameof(MoviesController.CreateMovie), new { }) ?? string.Empty, "create-movie", HttpMethod.Post.Method),
                new ResourceLink(Url.Link(nameof(RatingsController.GetRatingList), new { }) ?? string.Empty, "ratings", HttpMethod.Get.Method),
                new ResourceLink(Url.Link(nameof(RatingsController.CreateRating), new { }) ?? string.Empty, "create-rating", HttpMethod.Post.Method),
                new ResourceLink(Url.Link(nameof(UsersController.GetUserList), new { }) ?? string.Empty, "users", HttpMethod.Get.Method),
                new ResourceLink(Url.Link(nameof(UsersController.CreateUser), new { }) ?? string.Empty, "create-user", HttpMethod.Post.Method)
            };
            return Ok(links);
        }
    }
}
