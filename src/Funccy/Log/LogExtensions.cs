namespace Funccy
{
    public static class LogExtensions
    {
        public static Log<T, TLog> AsLog<T, TLog>(this T obj)
        {
            return new Log<T, TLog>(obj);
        }
    }
}
