using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BaseCompanyBll : BasicBll<Material_Base_Company>, IBaseCompanyBll
    {
        public override void SetBasicDal()
        {
            BasicDal = UnityContainerHelper.Server<IBaseCompanyDal>();
        }
    }
}