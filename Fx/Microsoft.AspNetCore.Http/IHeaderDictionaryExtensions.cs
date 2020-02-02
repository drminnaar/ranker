using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Http
{
    public static class IHeaderDictionaryExtensions
    {
        public static void AddPaginationHeader<T>(
            this IHeaderDictionary headers,
            IPagedCollection<T> items,
            string routeName,
            PagedQueryParams queryParams,
            IUrlHelper urlHelper)
        {
            if (headers is null)
                throw new ArgumentNullException(nameof(headers));

            if (items is null)
                throw new ArgumentNullException(nameof(items));

            if (string.IsNullOrEmpty(routeName))
                throw new ArgumentException("Expected non-null/empty route name", nameof(routeName));

            if (queryParams is null)
                throw new ArgumentNullException(nameof(queryParams));

            if (urlHelper is null)
                throw new ArgumentNullException(nameof(urlHelper));

            headers.Add(items.ToPaginationHeader(routeName, queryParams, urlHelper).ToKeyValuePair());
        }
    }
}
