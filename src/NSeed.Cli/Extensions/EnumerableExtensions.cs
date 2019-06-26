using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSeed.Cli.Extensions
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> AreEmptyIfNull<T>(this IEnumerable<T> source) =>
        source ?? Enumerable.Empty<T>();
    }
}
