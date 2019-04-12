using System.Linq;

namespace ZERO.Material.Command
{
    public static class StringExtension
    {
        public static string FirstToUpper(this string str)
        {
            return str.First().ToString().ToUpper() + str.Substring(1);
        }

        public static string FirstToLower(this string str)
        {
            return str.First().ToString().ToLower() + str.Substring(1);
        }
    }
}