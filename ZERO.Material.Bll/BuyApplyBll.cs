using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BuyApplyBll : BasicBll<Buy_Apply>, IBuyApplyBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.BuyApplyDal();
        }
    }
}