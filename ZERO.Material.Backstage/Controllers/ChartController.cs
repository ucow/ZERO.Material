using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;

namespace ZERO.Material.Backstage.Controllers
{
    public class ChartController : Controller
    {
        public ITypeBll _TypeBll = UnityContainerHelper.Server<ITypeBll>();

        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }
    }
}