using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class CompanyController : Controller
    {
        private ICompanyBll _companyBll = new UnityContainerHelper().Server<ICompanyBll>();

        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public string List(int page, int limit)
        {
            List<Material_Company> materialMessages = _companyBll.GetPageEntities(page, limit, (m => m.Company_Id), out var total);
            var dataJson = new
            {
                code = 0,
                msg = "OK",
                count = total,
                data = materialMessages
            };
            string json = JsonConvert.SerializeObject(dataJson);
            return json;
        }
    }
}