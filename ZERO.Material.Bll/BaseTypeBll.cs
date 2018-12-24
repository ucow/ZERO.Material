using ZERO.Material.Dal.Factory;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BaseTypeBll : BasicBll<Material_Base_Type>, IBaseTypeBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.BaseTypeDal();
        }
    }
}