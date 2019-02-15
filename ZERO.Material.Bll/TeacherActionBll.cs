using System.Collections.Generic;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class TeacherActionBll : BasicBll<Material_Teacher_Action>, ITeacherActionBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.TeacherActionDal();
        }

        /// <summary>
        /// 1. 删除原有的权限
        /// 2. 删除actionIds中该用户被分配的角色拥有权限的页面编号
        /// 3. 如果actionIds中没有该用户被分配的角色拥有权限的页面编号，将该页面编号添加到数据库中设置Has_Permission为false
        /// </summary>
        /// <param name="teacherId">用户编号</param>
        /// <param name="roleActionIds">用户被分配的角色拥有权限的页面编号</param>
        /// <param name="actionIds">用户被分配权限页面编号</param>
        /// <returns></returns>
        public bool SetTeacherAction(string teacherId, List<int> roleActionIds, List<int> actionIds)
        {
            //
            List<Material_Teacher_Action> deleteTeacherActions = GetEntities(m => m.Teacher_Id == teacherId);
            if (!DeleteEntity(deleteTeacherActions))
            {
                return false;
            }

            List<int> removeIds = new List<int>();

            foreach (var roleActionId in roleActionIds)
            {
                if (actionIds.Contains(roleActionId))
                {
                    removeIds.Add(roleActionId);
                }
            }

            roleActionIds.RemoveAll(m => removeIds.Contains(m));
            actionIds.RemoveAll(m => removeIds.Contains(m));

            List<Material_Teacher_Action> addTeacherActions = new List<Material_Teacher_Action>();
            foreach (int actionId in actionIds)
            {
                addTeacherActions.Add(new Material_Teacher_Action
                {
                    Action_Id = actionId,
                    Teacher_Id = teacherId,
                    Has_Permission = true
                });
            }

            foreach (int roleActionId in roleActionIds)
            {
                addTeacherActions.Add(new Material_Teacher_Action
                {
                    Action_Id = roleActionId,
                    Teacher_Id = teacherId,
                    Has_Permission = false
                });
            }

            if (AddEntities(addTeacherActions))
            {
                return true;
            }

            return false;
        }
    }
}