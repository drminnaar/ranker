using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Ranker.Application.Movies.Models
{
    public sealed class MovieQuery : PagedQueryParams
    {
        [FromQuery(Name = "title")]
        public string? Title { get; set; }

        [FromQuery(Name = "genres")]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))]
        public IReadOnlyCollection<string>? Genres { get; set; }

        [FromQuery(Name = "order")]
        public string? Order { get; set; }
    }
}
