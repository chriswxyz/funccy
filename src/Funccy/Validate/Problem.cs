namespace Funccy
{
    public class Problem<TErr>
    {
        public Problem(string key, TErr desc)
        {
            Key = key;
            Description = desc;
        }

        public string Key { get; }
        public TErr Description { get; }
    }
}
