using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Unity;

namespace ZERO.Material.Command
{
    public class UnityContainerHelper
    {
        private static readonly IUnityContainer Container = new UnityContainer();

        static UnityContainerHelper()
        {
            UnityConfigurationSection configuration =
                ConfigurationManager.GetSection(UnityConfigurationSection.SectionName) as UnityConfigurationSection;
            configuration?.Configure(Container, "Material");
        }

        public static T Server<T>()
        {
            return Container.Resolve<T>();
        }

        public static T Server<T>(string name)
        {
            return Container.Resolve<T>(name);
        }
    }
}