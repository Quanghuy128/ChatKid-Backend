namespace ChatKid.Common.Extensions
{
    public static class StringExtensions
    {
        public static string TrimEnd(this string str, string removeString)
        {
            str = str.TrimEnd();
            if (str.EndsWith(removeString))
            {
                str = str.Substring(0, str.LastIndexOf(removeString));
            }

            return str.TrimEnd();
        }
    }
}
