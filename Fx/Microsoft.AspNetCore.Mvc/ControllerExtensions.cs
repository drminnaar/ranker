using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerExtensions
    {
        public static IActionResult OkWithPageHeader<T>(
            this ControllerBase controller,
            IPagedCollection<T> value,
            string routeName,
            PagedQueryParams queryParams,
            IUrlHelper urlHelper)
        {
            if (controller is null)
                throw new ArgumentNullException(nameof(controller));

            if (value is null)
                throw new ArgumentNullException(nameof(value));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException("Expected non-null/empty route name", nameof(routeName));

            if (queryParams is null)
                throw new ArgumentNullException(nameof(queryParams));

            if (urlHelper is null)
                throw new ArgumentNullException(nameof(urlHelper));

            controller.Response.Headers.AddPaginationHeader(value, routeName, queryParams, urlHelper);
            return controller.Ok(value.ToList());
        }
    }
}
