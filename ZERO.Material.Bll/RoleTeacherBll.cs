using System.Collections.Generic;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class RoleTeacherBll : BasicBll<Material_Role_Teacher>, IRoleTeacherBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.RoleTeacherDal();
        }

        public bool SetTeacherRole(string teacherId, List<int> roleIds)
        {
            List<Material_Role_Teacher> deleteRoleTeachers = GetEntities(m => m.Teacher_Id == teacherId);
            if (!DeleteEntity(deleteRoleTeachers))
            {
                return false;
            }

            List<Material_Role_Teacher> addRoleTeachers = new List<Material_Role_Teacher>();
            foreach (int roleId in roleIds)
            {
                addRoleTeachers.Add(new Material_Role_Teacher
                {
                    Role_Id = roleId,
                    Teacher_Id = teacherId
                });
            }

            if (AddEntities(addRoleTeachers))
            {
                return true;
            }

            return false;
        }
    }
}