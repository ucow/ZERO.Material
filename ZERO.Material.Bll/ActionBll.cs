using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class ActionBll : BasicBll<Material_Action>, IActionBll
    {
        public override void SetBasicDal()
        {
            BasicDal = DbSession.ActionDal();
        }
    }
}