namespace Funccy
{
    public static class MaybeObjectExtensions
    {
        /// <summary>
        /// Turns a reference type object into a Maybe,
        /// based on if the object is null or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Maybe<T> AsMaybe<T>(this T obj) where T : class
        {
            return obj != null
                ? new Maybe<T>(obj)
                : new Maybe<T>();
        }

        /// <summary>
        /// Turns a nullable struct into a Maybe,
        /// based on if the struct has a value or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Maybe<T> AsMaybe<T>(this T? obj) where T : struct
        {
            return obj.HasValue
                ? new Maybe<T>(obj.Value)
                : new Maybe<T>();
        }
    }
}
