using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class CompanyController : BaseController<Material_Company>
    {
        private ICompanyBll _companyBll = UnityContainerHelper.Server<ICompanyBll>();

        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public string List(int page, int limit)
        {
            List<Material_Company> materialCompanies = _companyBll.GetPageEntities(page, limit, (m => m.Company_Id), (m => true), out var total);
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
            Material_Company materialCompany = _companyBll.Find(company_Id);
            if (materialCompany == null)
                return View();
            return View(materialCompany);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(Material_Company materialCompany, bool isUpdate)
        {
            if (isUpdate)
            {
                Material_Company company = _companyBll.Find(materialCompany.Company_Id);
                company.Company_Name = materialCompany.Company_Name;
                company.Company_Remark = materialCompany.Company_Remark;
                company.Company_Image = materialCompany.Company_Image;
                return _companyBll.UpdateEntities(new List<Material_Company>() { company }) ? Content("修改成功") : Content("修改失败");
            }
            else
            {
                return _companyBll.AddEntities(new List<Material_Company>() { materialCompany }) ? Content("添加成功") : Content("添加失败");
            }
        }

        public ActionResult Detail(string company_Id)
        {
            Material_Company materialCompany = _companyBll.Find(company_Id);
            return View(materialCompany);
        }

        public string Delete(string company_Id)
        {
            if (_companyBll.DeleteEntity(new List<Material_Company>()
            {
                _companyBll.Find( company_Id)
            }))
            {
                return "OK";
            }

            return "Error";
        }

        public FileContentResult GetImage(string id)
        {
            Material_Company company = _companyBll.Find(id);
            if (company != null && company.Company_Image != null)
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