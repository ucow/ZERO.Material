using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace ZERO.Material.Backstage.Controllers
{
    public class HomeController : Controller
    {
        private ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}