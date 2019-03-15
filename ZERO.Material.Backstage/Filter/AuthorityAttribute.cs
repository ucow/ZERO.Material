using System.Linq;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Filter
{
    public class AuthorityAttribute : ActionFilterAttribute
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

            if (managerInfo == null)
                return;

            var controller = (filterContext.RouteData.Values["controller"] as string)?.ToLower();
            var action = (filterContext.RouteData.Values["action"] as string)?.ToLower();
            if (controller == "zero" || controller == "error" || controller == "login")
                return;

            //..\Company\Index
            var actionUrl = $"..\\{controller}\\{action}";
            var materialAction = _actionBll.GetEntity(m => m.Action_Url == actionUrl);
            if (materialAction == null)
                return;

            Material_Teacher materialTeacher = _teacherBll.GetEntity(m => m.Teacher_Name == managerInfo || m.Teacher_Id == managerInfo);
            if (materialTeacher == null)
                return;
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