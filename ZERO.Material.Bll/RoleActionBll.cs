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
    }
}