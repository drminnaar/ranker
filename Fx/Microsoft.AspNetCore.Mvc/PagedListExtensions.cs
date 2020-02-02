using System;
using System.Collections.Generic;

namespace Microsoft.AspNetCore.Mvc
{
    public static class PagedListExtensions
    {
        public static PaginationHeader ToPaginationHeader<T>(
            this IPagedCollection<T> pagedCollection,
            string routeName,
            PagedQueryParams queryParams,
            IUrlHelper urlHelper)
        {
            if (pagedCollection is null)
                throw new ArgumentNullException(nameof(pagedCollection));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException("message", nameof(routeName));

            if (queryParams is null)
                throw new ArgumentNullException(nameof(queryParams));

            if (urlHelper is null)
                throw new ArgumentNullException(nameof(urlHelper));

            return new PaginationHeader(
                pagedCollection.CurrentPageNumber,
                pagedCollection.PageSize,
                pagedCollection.PageCount,
                pagedCollection.ItemCount,
                urlHelper.LinkNextPage(routeName, pagedCollection.PageSize, pagedCollection.NextPageNumber, queryParams),
                urlHelper.LinkPreviousPage(routeName, pagedCollection.PageSize, pagedCollection.PreviousPageNumber, queryParams),
                urlHelper.LinkFirstPage(routeName, pagedCollection.PageSize, queryParams),
                urlHelper.LinkLastPage(routeName, pagedCollection.PageSize, pagedCollection.LastPageNumber, queryParams),
                urlHelper.LinkCurrentPage(routeName, pagedCollection.PageSize, pagedCollection.CurrentPageNumber, queryParams));
        }
    }
}
