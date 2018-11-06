using ZERO.Material.IBll;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class ProductCompanyBll : BasicBll<Material_Product_Company>, IProductCompanyBll
    {
        public override void SetBasicDal()
        {
            BasicDal = UnityContainerHelper.Server<IProductCompanyDal>();
        }
    }
}