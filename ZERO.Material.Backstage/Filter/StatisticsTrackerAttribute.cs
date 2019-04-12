using System;
using System.Globalization;
using System.Web.Mvc;
using ZERO.Material.Command;

namespace ZERO.Material.Backstage.Filter
{
    /// <summary>
    /// 统计跟踪器
    /// 错误日志跟踪
    /// 权限处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class StatisticsTrackerAttribute : ActionFilterAttribute, IExceptionFilter
    {

        #region Action时间监控

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string msg =
                $"{filterContext.RequestContext.HttpContext.Request.UserHostName}:{filterContext.RequestContext.HttpContext.Request.UserHostAddress}在{Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff", DateTimeFormatInfo.InvariantInfo))}开始访问/{filterContext.RouteData.Values["controller"] as string}/{filterContext.RouteData.Values["action"] as string}";
            Log4NetHelper.Info(msg);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string msg =
                $"{filterContext.RequestContext.HttpContext.Request.UserHostName}:{filterContext.RequestContext.HttpContext.Request.UserHostAddress}在{Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff", DateTimeFormatInfo.InvariantInfo))}结束访问/{filterContext.RouteData.Values["controller"] as string}/{filterContext.RouteData.Values["action"] as string}";
            Log4NetHelper.Info(msg);
        }

        #endregion Action时间监控

        #region View 视图生成时间监控

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        #endregion View 视图生成时间监控

        #region 错误日志

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                string controllerName = string.Format("{0}Controller", filterContext.RouteData.Values["controller"] as string);
                string actionName = filterContext.RouteData.Values["action"] as string;
                string errorMsg = string.Format("{2}:{3}在执行 controller[{0}] 的 action[{1}] 时产生异常", controllerName, actionName, filterContext.RequestContext.HttpContext.Request.UserHostName, filterContext.RequestContext.HttpContext.Request.UserHostAddress);
                Log4NetHelper.Error(errorMsg, filterContext.Exception);
            }
        }

        #endregion 错误日志
    }
}