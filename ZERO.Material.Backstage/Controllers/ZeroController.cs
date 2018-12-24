using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;

namespace ZERO.Material.Backstage.Controllers
{
    public class ZeroController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string material)
        {
            return View();
        }
    }
}