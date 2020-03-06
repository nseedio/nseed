using GettingThingsDone.Contracts.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GettingThingsDone.Infrastructure.Database
{
    internal static class QueryableExtensions
    {
        public static IQueryable<T> WithTrackingOption<T>(this IQueryable<T> queryable, TrackingOption tracking)
            where T : class
        {
            return tracking == TrackingOption.WithTracking
                ? queryable.AsTracking()
                : queryable.AsNoTracking();
        }
    }
}
