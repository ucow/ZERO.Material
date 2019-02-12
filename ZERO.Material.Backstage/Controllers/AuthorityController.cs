using Newtonsoft.Json;
using System.Collections.Generic;
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

        public ActionResult AddAction()
        {
            ViewBag.parentMenu = _actionBll.GetEntities(m => m.Menu_Id == 0);
            return View();
        }

        [HttpPost]
        public string AddAction(Material_Action materialAction)
        {
            return _actionBll.AddEntities(new List<Material_Action>() { materialAction }) ? "添加成功" : "添加失败";
        }

        public string DeleteAction(int id)
        {
            return _actionBll.DeleteEntity(new List<Material_Action>() { _actionBll.Find(id) }) ? "OK" : "Error";
        }

        #endregion 页面管理
    }
}