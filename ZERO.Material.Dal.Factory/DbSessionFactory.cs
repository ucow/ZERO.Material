using ZERO.Material.Command;
using ZERO.Material.IDal;

namespace ZERO.Material.Dal.Factory
{
    public class DbSessionFactory
    {
        private static readonly UnityContainerHelper Container = new UnityContainerHelper();

        public static IDbSession GetDbSession()
        {
            return Container.Server<IDbSession>();
        }
    }
}