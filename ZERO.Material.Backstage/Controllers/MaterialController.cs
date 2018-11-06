using System.Collections.Generic;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class MaterialController : Controller
    {
        private IBaseBll _baseBll = new UnityContainerHelper().Server<IBaseBll>();

        // GET: Material
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public string Add(Material_Base materialBase)
        {
            if (_baseBll.AddEntity(new List<Material_Base>() { materialBase }))
            {
                return "OK";
            }

            return "Error";
        }
    }
}