using System.Collections.Generic;

namespace Chat.Server
{
    public static class Extensions
    {
        public static string ToJsonString<T>(this IEnumerable<T> list)
        {
            return $"{{{string.Join(", ", list)}}}";
        }
    }
}