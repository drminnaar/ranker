using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc
{
    [Serializable]
    public abstract class QueryParamsBase
    {
        protected QueryParamsBase()
        {
        }

        public virtual IDictionary<string, object> ToRouteValuesDictionary()
        {
            var routeValues = new Dictionary<string, object>();

            var properties = GetType()
               .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase)
               ?? Array.Empty<PropertyInfo>();

            foreach (var property in properties)
            {
                var value = property.GetValue(this);

                if (value == null)
                    continue;

                if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                    continue;

                if (!(property.GetCustomAttribute(typeof(FromQueryAttribute)) is FromQueryAttribute fromQueryAttribute))
                    continue;

                if (value is DateTimeOffset)
                {
                    routeValues.Add(fromQueryAttribute.Name, ((DateTimeOffset)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                }
                else if (value is DateTime)
                {
                    routeValues.Add(fromQueryAttribute.Name, ((DateTime)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                }
                else
                {
                    var key = fromQueryAttribute.Name ?? property.Name;
                    routeValues.Add(key, value);

                }
            }

            return routeValues;
        }
    }
}
