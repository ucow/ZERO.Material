using ZERO.Material.IBll;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class CompanyBll : BasicBll<Material_Company>, ICompanyBll
    {
        public override void SetBasicDal()
        {
            BasicDal = UnityContainerHelper.Server<ICompanyDal>();
        }
    }
}