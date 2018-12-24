using ZERO.Material.Dal.Factory;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BaseCompanyBll : BasicBll<Material_Base_Company>, IBaseCompanyBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.BaseCompanyDal();
        }
    }
}