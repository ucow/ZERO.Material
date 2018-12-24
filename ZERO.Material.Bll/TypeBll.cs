using ZERO.Material.Dal.Factory;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class TypeBll : BasicBll<Material_Type>, ITypeBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.TypeDal();
        }
    }
}