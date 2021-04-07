using System;
using System.Text;
using System.Text.Json;

namespace Microsoft.AspNetCore.Mvc
{
    public interface IETagProvider
    {
        string GetETag<T>(T type);
    }

    public sealed class ETagProvider : IETagProvider
    {
        public string GetETag<T>(T type)
        {
            var json = JsonSerializer.Serialize(type);
            var bytes = Encoding.UTF8.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }

    }
}
