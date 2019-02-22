using ZERO.Material.Command;
using ZERO.Material.IDal;

namespace ZERO.Material.Dal.Factory
{
    public class DbSessionFactory
    {
        public static IDbSession GetDbSession()
        {
            return UnityContainerHelper.Server<IDbSession>();
        }
    }
}