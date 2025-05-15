namespace Micro.Common
{
    public static class StringExtensions
    {
        // netstandard2.0 string doesn't have this method!?!
        public static string[] Split(this string str, string delimiter)
        {
            return str.Split(new[] { delimiter }, System.StringSplitOptions.None);
        }
    }
}
