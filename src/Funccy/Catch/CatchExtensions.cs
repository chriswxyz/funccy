namespace Funccy
{
    public static class CatchExtensions
    {
        public static CatchBuilder<TObj> AsCatch<TObj>(this TObj obj)
        {
            return new CatchBuilder<TObj>(obj);
        }
    }
}
