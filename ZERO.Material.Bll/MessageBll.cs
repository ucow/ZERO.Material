using ZERO.Material.IBll;
using ZERO.Material.IDal;
using ZERO.Material.Model;

namespace ZERO.Material.Bll
{
    public class MessageBll : BasicBll<Material_Message>, IMessageBll
    {
        public override void SetBasicDal()
        {
            BasicDal = UnityContainerHelper.Server<IMessageDal>();
        }
    }
}