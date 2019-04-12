using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Filter
{
    public class AuthorityFilterAttribute : ActionFilterAttribute
    {
        #region 全局变量

        private readonly IActionBll _actionBll = UnityContainerHelper.Server<IActionBll>();
        private readonly IRoleBll _roleBll = UnityContainerHelper.Server<IRoleBll>();
        private readonly IRoleActionBll _roleActionBll = UnityContainerHelper.Server<IRoleActionBll>();
        private readonly ITeacherActionBll _teacherActionBll = UnityContainerHelper.Server<ITeacherActionBll>();
        private readonly ITeacherBll _teacherBll = UnityContainerHelper.Server<ITeacherBll>();
        private readonly IRoleTeacherBll _roleTeacherBll = UnityContainerHelper.Server<IRoleTeacherBll>();

        #endregion 全局变量

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var managerInfo = filterContext.RequestContext.HttpContext.Request.Cookies["managerInfo"]?.Value;

            if (managerInfo == "null" || managerInfo == null)
                return;

            var controller = (filterContext.RouteData.Values["controller"] as string).FirstToUpper();
            var action = (filterContext.RouteData.Values["action"] as string).FirstToUpper();
            if (controller == "zero" || controller == "error" || controller == "login")
                return;
            var url = filterContext.HttpContext.Request.Url.ToString();
            //..\Company\Index

            var actionUrl = string.Format("..\\{0}\\{1}", controller, action);
            
            var materialActions = _actionBll.GetEntities(m => m.Action_Url == actionUrl);
            if (materialActions == null || materialActions.Count == 0)
            {
                return;
            }
            var materialAction = _actionBll.GetEntity(m => m.Action_Url == actionUrl);
            if (materialAction == null)
                return;
            
            Material_Teacher materialTeacher = _teacherBll.GetEntity(m => m.Teacher_Name == managerInfo || m.Teacher_Id == managerInfo);
            
            var roleIds = _roleTeacherBll.GetEntities(m => m.Teacher_Id == materialTeacher.Teacher_Id).Select(m => m.Role_Id).ToList();
            
            var actionIds = _roleActionBll.GetEntities(m => roleIds.Contains(m.Role_Id)).Select(m => m.Action_Id).ToList();
            if (actionIds.Contains(materialAction.Id))
            {
                return;
            }
            actionIds.AddRange(_teacherActionBll.GetEntities(m => m.Teacher_Id == materialTeacher.Teacher_Id).Select(m => m.Action_Id).ToList());
            if (!actionIds.Contains(materialAction.Id))
            {
                filterContext.Result = new RedirectResult("..\\Error\\NoAuthority");
            }
        }
    }
}