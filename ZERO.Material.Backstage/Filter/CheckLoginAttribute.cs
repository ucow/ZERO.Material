using System.Web.Mvc;

namespace ZERO.Material.Backstage.Filter
{
    public class CheckLoginAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies["userInfo"] == null ||
                filterContext.HttpContext.Request.Cookies["userInfo"]?.Value == "null")
            {
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
        }
    }
}