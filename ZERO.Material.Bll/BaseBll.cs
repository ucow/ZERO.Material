using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class BaseBll : BasicBll<Material_Base>, IBaseBll
    {
        public override void SetBasicDal()
        {
            BasicDal = UnityContainerHelper.Server<IBaseDal>();
        }
    }
}