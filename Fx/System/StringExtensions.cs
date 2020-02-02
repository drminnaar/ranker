using System.ComponentModel;
using System.Globalization;

namespace System
{
    public static class StringExtensions
    {
        [Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "The intention is to convert without errors")]
        public static T ConvertTo<T>(this string value) where T : struct, IComparable<T>
        {
            try
            {
                if (Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
                }

                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return default;
            }
        }
    }
}