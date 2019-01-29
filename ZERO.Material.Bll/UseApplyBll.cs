using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class UseApplyBll : BasicBll<Use_Apply>, IUseApplyBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.UseApplyDal();
        }
    }
}