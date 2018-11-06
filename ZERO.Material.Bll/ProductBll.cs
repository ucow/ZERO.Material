using ZERO.Material.IBll;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class ProductBll : BasicBll<Material_Product>, IProductBll
    {
        public override void SetBasicDal()
        {
            BasicDal = UnityContainerHelper.Server<IProductDal>();
        }
    }
}