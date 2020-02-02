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
                new ResourceLink(Url.Link(nameof(RootController.Get), new { }), "self", HttpMethod.Get.Method),
                new ResourceLink(Url.Link(nameof(MoviesController.GetMovieList), new { }), "movies", HttpMethod.Get.Method),
                new ResourceLink(Url.Link(nameof(MoviesController.CreateMovie), new { }), "create-movie", HttpMethod.Post.Method),
                new ResourceLink(Url.Link(nameof(RatingsController.GetRatingList), new { }), "ratings", HttpMethod.Get.Method),
                new ResourceLink(Url.Link(nameof(RatingsController.CreateRating), new { }), "create-rating", HttpMethod.Post.Method),
                new ResourceLink(Url.Link(nameof(UsersController.GetUserList), new { }), "users", HttpMethod.Get.Method),
                new ResourceLink(Url.Link(nameof(UsersController.CreateUser), new { }), "create-user", HttpMethod.Post.Method)
            };
            return Ok(links);
        }
    }
}
