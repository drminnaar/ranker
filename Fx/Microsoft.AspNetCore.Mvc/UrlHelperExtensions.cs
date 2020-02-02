using System;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static Uri? LinkCurrentPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            int currentPageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (currentPageNumber < 1 || pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, currentPageNumber, queryParams);
        }

        public static Uri? LinkFirstPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            T queryParams) where T : PagedQueryParams
        {
            if (pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, 1, queryParams);
        }

        public static Uri? LinkLastPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            int lastPageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (lastPageNumber < 1 || pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, lastPageNumber, queryParams);
        }

        public static Uri? LinkNextPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            int? nextPageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (!nextPageNumber.HasValue || nextPageNumber < 1 || pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, nextPageNumber.Value, queryParams);
        }

        public static Uri? LinkPreviousPage<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageSize,
            int? previousPageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (!previousPageNumber.HasValue || previousPageNumber < 1 || pageSize < 1)
                return null;

            return urlHelper.Link<T>(routeName, previousPageNumber.Value, queryParams);
        }

        public static Uri Link<T>(
            this IUrlHelper urlHelper,
            string routeName,
            int pageNumber,
            T queryParams) where T : PagedQueryParams
        {
            if (urlHelper is null)
                throw new ArgumentNullException(nameof(urlHelper));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException("Expected non-null/empty route name", nameof(routeName));

            if (queryParams is null)
                throw new ArgumentNullException(nameof(queryParams));

            if (!(typeof(T)
                .GetProperty(nameof(PagedQueryParams.Page), BindingFlags.Public | BindingFlags.Instance)
                ?.GetCustomAttribute(typeof(FromQueryAttribute)) is FromQueryAttribute pageNumberParam))
                throw new InvalidOperationException($"Expected the property '{nameof(PagedQueryParams.Page)}' to have an attribute of type '{nameof(FromQueryAttribute)}'.");

            var routeValues = queryParams.ToRouteValuesDictionary();
            routeValues[pageNumberParam.Name] = pageNumber;

            return new Uri(urlHelper.Link(routeName, routeValues));
        }
    }
}
