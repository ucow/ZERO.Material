using System.Web;
using System.Web.Mvc;

namespace ZERO.Material.Backstage
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
