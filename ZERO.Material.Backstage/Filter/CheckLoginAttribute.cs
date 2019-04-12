using System.Web;
using System.Web.Mvc;

namespace ZERO.Material.Backstage.Filter
{
    public class CheckLoginAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public bool IsChecked { get; set; }

        public  void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!IsChecked) return;

            if (filterContext.RouteData.Values["controller"] is string controller && controller.ToLower() == "zero")
            {
                if (filterContext.HttpContext.Request.Cookies["userInfo"] != null &&
                    filterContext.HttpContext.Request.Cookies["userInfo"]?.Value != "null") return;

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    ContentResult content = new ContentResult { Content = "请先登录" };
                    filterContext.Result = content;
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Login/BeforeLogin");
                }
            }
            else
            {
                if (filterContext.HttpContext.Request.Cookies["managerInfo"] != null &&
                    filterContext.HttpContext.Request.Cookies["managerInfo"]?.Value != "null") return;

                filterContext.Result = new RedirectResult("/Login/BackstageLogin");
            }
        }
    }
}