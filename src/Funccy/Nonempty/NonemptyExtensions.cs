using System.Collections.Generic;

namespace Funccy
{
    public static class NonemptyExtensions
    {
        /// <summary>
        /// Converts to a nonempty list, if there is at least one item
        /// in the source collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static Maybe<NonemptyList<T>> AsNonemptyList<T>(this IEnumerable<T> coll)
        {
            var first = coll.FirstMaybe();
            var rest = coll.Rest();

            return first.Map(x => new NonemptyList<T>(x, rest));
        }
    }
}
