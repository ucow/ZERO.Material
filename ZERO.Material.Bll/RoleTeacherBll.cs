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
    }
}