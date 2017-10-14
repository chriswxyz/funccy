namespace Funccy
{
    public class Problem
    {
        public Problem(string key, string desc)
        {
            Key = key;
            Description = desc;
        }

        public string Key { get; }
        public string Description { get; }
    }
}
