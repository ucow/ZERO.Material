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

        public string GetModelInfo()
        {
            return AssmblyHelper.GetDisplayAttributeInfo<Material_Company>();
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

        public ActionResult Add(string company_Id)
        {
            if (string.IsNullOrEmpty(company_Id))
            {
                return View();
            }

            Material_Company materialCompany = _companyBll.GetEntity(m => m.Company_Id == company_Id);
            if (materialCompany == null)
                return View();
            return View(materialCompany);
        }

        [HttpPost]
        public ActionResult Add(Material_Company materialCompany)
        {
            if (_companyBll.AddOrUpdateEntity(new List<Material_Company>() { materialCompany }))
            {
                return Content("OK");
            }

            return Content("Error");
        }

        public ActionResult Detail(string company_Id)
        {
            Material_Company materialCompany = _companyBll.GetEntity(m => m.Company_Id == company_Id);
            return View(materialCompany);
        }

        public string Delete(string company_Id)
        {
            if (_companyBll.DeleteEntity(new List<Material_Company>()
            {
                _companyBll.GetEntity(m=>m.Company_Id == company_Id)
            }))
            {
                return "OK";
            }

            return "Error";
        }
    }
}