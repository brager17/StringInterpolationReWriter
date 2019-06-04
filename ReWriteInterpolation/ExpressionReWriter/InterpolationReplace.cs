using System.Linq;

namespace Tests
{
    public static class InterpolationReplace
    {
        public static IQueryable<T> ReWrite<T>(this IQueryable<T> qu)
        {
            var result = new InterpolationStringReplacer<T>().Visit(qu.Expression);
            var s = (IQueryable<T>) qu.Provider.CreateQuery(result);
            return s;
        }
    }
}