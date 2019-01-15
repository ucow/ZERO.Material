namespace ZERO.Material.Command
{
    public class UrlHelper
    {
        public static string DecodeUrl(string src)
        {
            return System.Web.HttpUtility.UrlDecode(src);
        }

        public static string EncodeUrl(string src)
        {
            return System.Web.HttpUtility.UrlEncode(src);
        }
    }
}