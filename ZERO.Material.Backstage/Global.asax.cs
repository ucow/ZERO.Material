using System.IO;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            new ZeroMaterialContext().Database.CreateIfNotExists();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}