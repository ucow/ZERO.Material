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
            //验证登录
            filters.Add(new CheckLoginAttribute() { IsChecked = true });

            //权限验证
            filters.Add(new AuthorityFilterAttribute());
            
            //记录日志
            filters.Add(new StatisticsTrackerAttribute());
        }
    }
}