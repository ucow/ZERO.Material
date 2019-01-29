using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class ApplyTypeBll : BasicBll<Apply_Type>, IApplyTypeBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.ApplyTypeDal();
        }
    }
}