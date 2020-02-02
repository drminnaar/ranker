using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Microsoft.AspNetCore.Mvc
{
    public sealed class PaginationHeader
    {
        public PaginationHeader(
            int currentPageNumber,
            int pageSize,
            int pageCount,
            int itemCount,
            Uri? nextPageUrl,
            Uri? previousPageUrl,
            Uri? firstPageUrl,
            Uri? lastPageUrl,
            Uri? currentPageUrl)
        {
            CurrentPageNumber = currentPageNumber;
            PageSize = pageSize;
            PageCount = pageCount;
            ItemCount = itemCount;
            NextPageUrl = nextPageUrl;
            PreviousPageUrl = previousPageUrl;
            FirstPageUrl = firstPageUrl;
            LastPageUrl = lastPageUrl;
            CurrentPageUrl = currentPageUrl;
        }

        public int CurrentPageNumber { get; }
        public int ItemCount { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public Uri? FirstPageUrl { get; }
        public Uri? LastPageUrl { get; }
        public Uri? NextPageUrl { get; }
        public Uri? PreviousPageUrl { get; }
        public Uri? CurrentPageUrl { get; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public KeyValuePair<string, StringValues> ToKeyValuePair()
        {
            return new KeyValuePair<string, StringValues>("X-Pagination", this.ToJsonString());
        }
    }
}
