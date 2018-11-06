using ZERO.Material.IBll;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class TypeBll : BasicBll<Material_Type>, ITypeBll
    {
        public override void SetBasicDal()
        {
            BasicDal = UnityContainerHelper.Server<ITypeDal>();
        }
    }
}