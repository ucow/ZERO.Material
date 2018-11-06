using System.Configuration;
using Microsoft.Practices.Unity.Configuration;
using Unity;

namespace ZERO.Material.Command
{
    public class UnityContainerHelper
    {
        private IUnityContainer container = new UnityContainer();

        public UnityContainerHelper()
        {
            UnityConfigurationSection configuration =
                ConfigurationManager.GetSection(UnityConfigurationSection.SectionName) as UnityConfigurationSection;
            configuration?.Configure(container, "Material");
        }

        public T Server<T>()
        {
            return container.Resolve<T>();
        }

        public T Server<T>(string name)
        {
            return container.Resolve<T>(name);
        }
    }
}