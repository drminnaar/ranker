using System;

namespace Microsoft.AspNetCore.Mvc
{
    [Serializable]
    public class PagedQueryParams : QueryParamsBase
    {
        private const int DEFAULT_PAGE_NUMBER = 1;
        private const int DEFAULT_PAGE_SIZE = 10;

        public PagedQueryParams() : base()
        {
        }

        /// <summary>
        /// A page number having a numeric value of 1 or greater
        /// </summary>
        [FromQuery(Name = "page")]
        public int Page { get; set; } = DEFAULT_PAGE_NUMBER;

        /// <summary>
        /// A page size having a numeric value of 1 or greater.static Represents the number of tracks returned per page.
        /// </summary>
        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = DEFAULT_PAGE_SIZE;
    }
}
