namespace JarleF.NAnt.Tasks
{
    public static class StringExtensions
    {
        public static string FormatString(this string text, params object[] args)
        {
            return string.Format(text, args);
        }
    }
}
