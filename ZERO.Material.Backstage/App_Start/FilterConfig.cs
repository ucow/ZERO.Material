using System.Web;
using System.Web.Mvc;
using ZERO.Material.Backstage.Filter;

namespace ZERO.Material.Backstage
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new StatisticsTrackerAttribute());
        }
    }
}