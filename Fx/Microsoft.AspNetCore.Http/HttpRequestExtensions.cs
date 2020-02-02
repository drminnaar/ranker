using System;
using System.Linq;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        public static string? GetETagHeader(this HttpRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return request.Headers["If-None-Match"].FirstOrDefault();
        }

        public static bool HasETagHeader(this HttpRequest request)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            return request.Headers.ContainsKey("If-None-Match");
        }

    }
}
