using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZERO.Material.Command;

namespace ZERO.Material.Backstage.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Before()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Before(string username, string password)
        {
            if (username == "admin" && password == "admin")
            {
                return Content("OK");
            }
            return Content("Error");
        }

        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}