using System.Reflection;

namespace Pharmacy.Application.Extensions
{
    public static class StringExtensions
    {
        public static string KeepOld(this string? newValue, string oldValue)
        {
            return string.IsNullOrWhiteSpace(newValue)
                ? oldValue
                : newValue;
        }
    }
}