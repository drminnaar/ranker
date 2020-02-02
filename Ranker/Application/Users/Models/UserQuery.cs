using Microsoft.AspNetCore.Mvc;

namespace Ranker.Application.Users.Models
{
    public sealed class UserQuery : PagedQueryParams
    {
        [FromQuery(Name = "email")]
        public string? Email { get; set; }

        [FromQuery(Name = "first-name")]
        public string? FirstName { get; set; }

        [FromQuery(Name = "gender")]
        public string? Gender { get; set; }

        [FromQuery(Name = "last-name")]
        public string? LastName { get; set; }

        [FromQuery(Name = "min-age")]
        public string? MinimumAge { get; set; }

        [FromQuery(Name = "max-age")]
        public string? MaximumAge { get; set; }

        [FromQuery(Name = "order")]
        public string? Order { get; set; }
    }
}
