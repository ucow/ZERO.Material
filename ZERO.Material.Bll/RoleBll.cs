using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class RoleBll : BasicBll<Material_Role>, IRoleBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.RoleDal();
        }
    }
}