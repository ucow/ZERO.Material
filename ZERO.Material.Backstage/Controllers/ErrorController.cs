using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZERO.Material.Backstage.Filter;

namespace ZERO.Material.Backstage.Controllers
{
    [CheckLogin(IsChecked = false)]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult NoAuthority()
        {
            return View();
        }

        public ActionResult NoFound()
        {
            return View();
        }
    }
}