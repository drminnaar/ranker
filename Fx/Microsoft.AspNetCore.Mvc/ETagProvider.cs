using System;
using System.Text;
using System.Text.Json;

namespace Fx.Microsoft.AspNetCore.Mvc
{
    public sealed class ETagProvider
    {
        public string GetETag<T>(T type)
        {
            var json = JsonSerializer.Serialize(type);
            var bytes = Encoding.UTF8.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }

    }
}
