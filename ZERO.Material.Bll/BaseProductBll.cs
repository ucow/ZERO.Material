using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BaseProductBll : BasicBll<Material_Base_Product>, IBaseProductBll
    {
        public override void SetBasicDal()
        {
            BasicDal = UnityContainerHelper.Server<IBaseProductDal>();
        }
    }
}