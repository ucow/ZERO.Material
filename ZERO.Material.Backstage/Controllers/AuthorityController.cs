using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Web.Razor.Generator;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;
using ZERO.Material.Model.Other;

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

        public string DeleteRole(int id, bool isDel)
        {
            Material_Role role = _roleBll.Find(id);
            role.Del_Flag = isDel;
            return _roleBll.UpdateEntities(new List<Material_Role>() { role }) ? "OK" : "Error";
        }

        public ActionResult AddRole(int? id)
        {
            if (id == null)
            {
                return View();
            }

            ViewBag.actionRole = _roleActionBll.GetEntities(m => m.Action_Id == id).Select(m => m.Role_Id).ToList();
            return View(_roleBll.Find(id));
        }

        [HttpPost]
        public string AddRole(Material_Role materialRole, List<int> actions)
        {
            Material_Role updateRole = _roleBll.Find(materialRole.Id);
            if (updateRole == null)
            {
                return _roleBll.AddEntities(new List<Material_Role> { materialRole }) &&
                       _roleActionBll.SetActionByRole(actions, materialRole.Id) ? "添加成功" : "添加失败";
            }
            else
            {
                AssmblyHelper.ClassEvaluate(materialRole, updateRole);
                return _roleBll.UpdateEntities(new List<Material_Role> { updateRole }) && _roleActionBll.SetActionByRole(actions, updateRole.Id) ? "更新成功" : "更新失败";
            }
        }

        public ActionResult SetRoleAction()
        {
            return View();
        }

        [HttpPost]
        public string GetActionTree()
        {
            List<Material_Action> materialActions = _actionBll.GetEntities(m => m.Del_Flag == false);
            List<ActionTree> trees = new List<ActionTree>();
            foreach (Material_Action materialAction in materialActions)
            {
                //父节点
                if (materialAction.Menu_Id == 0)
                {
                    if (trees.FirstOrDefault(m => m.Value == materialAction.Id) == null)
                    {
                        trees.Add(new ActionTree
                        {
                            Title = materialAction.Action_Name,
                            Value = materialAction.Id,
                            Checked = false,
                            Disabled = false
                        });
                    }
                }
                else//子节点
                {
                    //查找父节点
                    ActionTree tree = trees.FirstOrDefault(m => m.Value == materialAction.Menu_Id);
                    //没有找到父节点 添加父节点
                    if (tree == null)
                    {
                        Material_Action action = materialActions.Find(m => m.Id == materialAction.Menu_Id);
                        tree = new ActionTree
                        {
                            Title = action.Action_Name,
                            Value = action.Id,
                            Checked = false,
                            Disabled = false
                        };
                        trees.Add(tree);
                    }

                    if (tree.Data == null)
                    {
                        tree.Data = new List<ActionTree>
                        {
                            new ActionTree
                            {
                                Title = materialAction.Action_Name,
                                Value = materialAction.Id,
                                Checked = false,
                                Disabled = false,
                                Data = new List<ActionTree>()
                            }
                        };
                    }
                    else
                    {
                        tree.Data.Add(new ActionTree
                        {
                            Title = materialAction.Action_Name,
                            Value = materialAction.Id,
                            Checked = false,
                            Disabled = false,
                            Data = new List<ActionTree>()
                        });
                    }
                }
            }

            var dataTrees = new
            {
                code = 0,
                data = trees,
                msg = "获取成功"
            };
            return JsonConvert.SerializeObject(dataTrees);
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