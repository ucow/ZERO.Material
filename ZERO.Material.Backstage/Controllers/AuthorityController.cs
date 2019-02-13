using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class AuthorityController : Controller
    {
        #region 全局变量

        private static readonly UnityContainerHelper Container = new UnityContainerHelper();
        private readonly IRoleBll _roleBll = Container.Server<IRoleBll>();
        private readonly IActionBll _actionBll = Container.Server<IActionBll>();
        private readonly IRoleActionBll _roleActionBll = Container.Server<IRoleActionBll>();

        #endregion 全局变量

        #region 角色管理

        // GET: Authority
        public ActionResult Role()
        {
            return View();
        }

        public string GetMaterialRole(int page, int limit)
        {
            List<Material_Role> roles = _roleBll.GetPageEntities(page, limit, m => m.Id, out int total);
            var msg = new
            {
                code = 0,
                msg = "",
                total = total,
                data = roles
            };
            return JsonConvert.SerializeObject(msg);
        }

        #endregion 角色管理

        #region 页面管理

        public ActionResult Action()
        {
            List<Material_Role> roles = _roleBll.GetEntities(m => m.Del_Flag == false);
            ViewBag.roles = roles;
            return View();
        }

        public string GetMaterialAction()
        {
            List<Material_Action> actions = new List<Material_Action>();
            List<Material_Action> menuActions = _actionBll.GetEntities(m => m.Menu_Id == 0);
            foreach (Material_Action menuAction in menuActions)
            {
                menuAction.Menu_Id = -1;
                actions.Add(menuAction);
                actions.AddRange(_actionBll.GetEntities(m => m.Menu_Id == menuAction.Id));
            }

            var msg = new
            {
                code = 0,
                msg = "",
                total = actions.Count,
                data = actions
            };
            return JsonConvert.SerializeObject(msg);
        }

        public ActionResult AddAction(int? id)
        {
            List<Material_Action> actions = _actionBll.GetEntities(m => m.Menu_Id == 0);
            List<Material_Role> roles = _roleBll.GetEntities(m => m.Del_Flag == false);
            ViewBag.roles = roles;
            actions.Add(new Material_Action()
            {
                Action_Name = "无",
                Id = 0,
                Menu_Id = 0
            });

            ViewBag.parentMenu = new SelectList(actions, "Id", "Action_Name");

            //
            if (id == null)
            {
                return View();
            }

            ViewBag.actionRole = _roleActionBll.GetEntities(m => m.Action_Id == id).Select(m => m.Role_Id).ToList();
            return View(_actionBll.Find(id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public string AddAction(Material_Action materialAction, int[] roles)
        {
            Material_Action action = _actionBll.Find(materialAction.Id);
            if (action == null)
            {
                return _actionBll.AddEntities(new List<Material_Action>() { materialAction }) && _roleActionBll.SetRoleByAction(roles.ToList(), materialAction.Id) ? "添加成功" : "添加失败";
            }
            else
            {
                AssmblyHelper.ClassEvaluate(materialAction, action);
                return _actionBll.UpdateEntities(new List<Material_Action>() { action }) && _roleActionBll.SetRoleByAction(roles.ToList(), materialAction.Id) ? "更新成功" : "更新失败";
            }
        }

        public string DeleteAction(int id, bool isDel)
        {
            Material_Action action = _actionBll.Find(id);
            action.Del_Flag = isDel;
            return _actionBll.UpdateEntities(new List<Material_Action>() { action }) ? "OK" : "Error";
        }

        public string SetMenu(int id, bool isMenu)
        {
            Material_Action action = _actionBll.Find(id);
            action.Is_Menu = isMenu;
            return _actionBll.UpdateEntities(new List<Material_Action>() { action }) ? "OK" : "Error";
        }

        public string GetActionRole(int id)
        {
            List<int> roleIds = _roleActionBll.GetEntities(m => m.Action_Id == id).Select(m => m.Role_Id).ToList();
            List<string> roleNames = _roleBll.GetEntities(m => roleIds.Contains(m.Id) && m.Del_Flag == false).Select(m => m.Role_Name).ToList();
            return JsonConvert.SerializeObject(roleNames);
        }

        public string SetActionRole(List<int> ids, int actionId)
        {
            return _roleActionBll.SetRoleByAction(ids, actionId) ? "OK" : "Error";
        }

        #endregion 页面管理
    }
}