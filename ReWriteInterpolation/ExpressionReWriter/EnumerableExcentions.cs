using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    // ReSharper disable  InvalidXmlDocComment
    public static class EnumerableExtension 
    {
        /// <summary>
        ///  Example: e1 = [1,33,5,7,9]; e2 = [2,4,6,88,10,0] ->[1,2,33,4,5,6,7,88,9,10,0]
        /// </summary>
        /// <param name="comparer">Если generic ссылочный тип, то создайте для него comparer</param>
        public static IEnumerable<T> Merge<T>(this IEnumerable<T> e1, IEnumerable<T> e2,
            IEqualityComparer<T> comparer = null)
        {
            var zip = e1.Zip(e2, (x, y) => new {x, y});

            var exc1 = e1.Except(zip.Select(x => x.x), comparer);
            var exc2 = e2.Except(zip.Select(x => x.y), comparer);

            foreach (var z in zip)
            {
                yield return z.x;
                yield return z.y;
            }

            foreach (var ex in exc1)
            {
                yield return ex;
            }

            foreach (var ex in exc2)
            {
                yield return ex;
            }
        }
    }
}