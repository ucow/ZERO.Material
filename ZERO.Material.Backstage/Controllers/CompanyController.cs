using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class CompanyController : BaseController<Material_Company>
    {
        private ICompanyBll _companyBll = new UnityContainerHelper().Server<ICompanyBll>();

        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public string List(int page, int limit)
        {
            List<Material_Company> materialCompanies = _companyBll.GetPageEntities(page, limit, (m => m.Company_Id), out var total);
            var dataJson = new
            {
                code = 0,
                msg = "OK",
                count = total,
                data = materialCompanies
            };
            string json = JsonConvert.SerializeObject(dataJson);
            return json;
        }

        public ActionResult Add(string company_Id)
        {
            if (string.IsNullOrEmpty(company_Id))
            {
                ViewBag.IsUpdate = false;
                return View();
            }
            ViewBag.IsUpdate = true;
            Material_Company materialCompany = _companyBll.GetEntity(m => m.Company_Id == company_Id);
            if (materialCompany == null)
                return View();
            return View(materialCompany);
        }

        [HttpPost]
        public ActionResult Add(Material_Company materialCompany, bool isUpdate)
        {
            if (isUpdate)
            {
                Material_Company company = _companyBll.Find(materialCompany.Company_Id);
                company.Company_Name = materialCompany.Company_Name;
                company.Company_Remark = materialCompany.Company_Remark;
                return _companyBll.UpdateEntities(new List<Material_Company>() { company }) ? Content("OK") : Content("Error");
            }
            else
            {
                return _companyBll.AddEntities(new List<Material_Company>() { materialCompany }) ? Content("OK") : Content("Error");
            }
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

        public FileContentResult GetImage(string id)
        {
            Material_Company company = _companyBll.Find(id);
            if (company != null)
            {
                return new FileContentResult(company.Company_Image, "Image/jpg");
            }
            else
            {
                return null;
            }
        }
    }
}