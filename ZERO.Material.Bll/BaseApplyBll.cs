using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BaseApplyBll : BasicBll<Material_Apply>, IBaseApplyBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.BaseApplyDal();
        }
    }
}