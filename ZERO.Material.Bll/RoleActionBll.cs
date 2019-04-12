using System.Collections.Generic;
using System.Linq;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class RoleActionBll : BasicBll<Material_Role_Action>, IRoleActionBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.RoleActionDal();
        }


        //1. 首先删除原有权限，然后重新分配权限
        //2. 如果该页面是子页面，在给自己分配权限的同时需要为父菜单项分配权限
        //3. 如果该页面是父菜单项，但是没有分配权限，需要将子页面中的所有权限删除
        //4. 如果该页面是父菜单项，并且分配了权限，在给自己分配权限的同时，需要判断子页面的权限，如果子页面拥有的权限多余父菜单项，这需要将多余的权限删除。
        public bool SetRoleByAction(List<int> roleIds, int actionId)
        {
            IActionBll actionBll = UnityContainerHelper.Server<IActionBll>();
            //删除该页面原有的角色权限
            List<Material_Role_Action> roleActions = GetEntities(m => m.Action_Id == actionId);
            if (!DeleteEntity(roleActions))
            {
                return false;
            }

            //重新分配角色权限
            List<Material_Role_Action> addRoleActions = new List<Material_Role_Action>();
            if (roleIds != null)
            {
                //获取该页面的父菜单Id
                Material_Action action = actionBll.Find(actionId);
                if (action == null)
                {
                    return false;
                }

                int menuId = action.Menu_Id;
                foreach (int roleId in roleIds)
                {
                    //为该页面设置权限
                    addRoleActions.Add(new Material_Role_Action
                    {
                        Role_Id = roleId,
                        Action_Id = actionId
                    });
                    Material_Role_Action roleAction = GetEntity(m => m.Action_Id == menuId && m.Role_Id == roleId);
                    //menuId != 0即拥有父菜单，并且该角色没有父菜单的权限则为该角色分配权限
                    if (menuId != 0 && roleAction == null)
                    {
                        addRoleActions.Add(new Material_Role_Action
                        {
                            Role_Id = roleId,
                            Action_Id = menuId
                        });
                    }
                }
                //如果是父菜单，判断子页面拥有的角色是否多于父菜单 将多出的角色权限删除
                if (menuId == 0)
                {
                    List<int> ids = actionBll.GetEntities(m => m.Menu_Id == actionId).Select(m => m.Id).ToList();
                    List<Material_Role_Action> deleteRoleActions = new List<Material_Role_Action>();
                    foreach (int id in ids)
                    {
                        List<int> roleIdList = GetEntities(m => m.Action_Id == id).Select(m => m.Role_Id).ToList();
                        foreach (int roleId in roleIdList)
                        {
                            if (!roleIds.Contains(roleId))
                            {
                                deleteRoleActions.Add(GetEntity(m => m.Action_Id == id && m.Role_Id == roleId));
                            }
                        }
                    }

                    DeleteEntity(deleteRoleActions);
                }
            }
            else
            {
                //如果该页面是菜单并且没有为任何角色分配权限，则将该菜单项的子页面的所有角色权限删除
                if (actionBll.Find(actionId).Is_Menu)
                {
                    List<int> ids = actionBll.GetEntities(m => m.Menu_Id == actionId).Select(m => m.Id).ToList();
                    if (!DeleteEntity(GetEntities(m => ids.Contains(m.Action_Id))))
                    {
                        return false;
                    }
                }
                else//如果该页面不是菜单且没有分配权限则结束操作
                {
                    return true;
                }
            }

            //添加权限
            if (AddEntities(addRoleActions))
            {
                return true;
            }

            return false;
        }

        // 删除原有权限重新分配权限
        public bool SetActionByRole(List<int> actionIds, int roleId)
        {
            List<Material_Role_Action> roleActions = GetEntities(m => m.Role_Id == roleId);
            if (!DeleteEntity(roleActions))
            {
                return false;
            }

            List<Material_Role_Action> addRoleActions = new List<Material_Role_Action>();
            foreach (int actionId in actionIds)
            {
                addRoleActions.Add(new Material_Role_Action
                {
                    Role_Id = roleId,
                    Action_Id = actionId
                });
            }

            if (AddEntities(addRoleActions))
            {
                return true;
            }

            return true;
        }
    }
}