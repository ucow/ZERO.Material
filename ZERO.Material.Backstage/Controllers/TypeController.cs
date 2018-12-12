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
    public class TypeController : Controller
    {
        private ITypeBll _companyBll = new UnityContainerHelper().Server<ITypeBll>();

        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public string List(int page, int limit)
        {
            List<Material_Type> materialMessages = _companyBll.GetPageEntities(page, limit, (m => m.Material_Type_Id), out var total);
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

        public string GetModelInfo()
        {
            return AssmblyHelper.GetDisplayAttributeInfo<Material_Type>();
        }

        public ActionResult Add(string Material_Type_Id)
        {
            if (string.IsNullOrEmpty(Material_Type_Id))
            {
                return View();
            }

            Material_Type materialCompany = _companyBll.GetEntity(m => m.Material_Type_Id == Material_Type_Id);
            if (materialCompany == null)
                return View();
            return View(materialCompany);
        }

        [HttpPost]
        public ActionResult Add(Material_Type materialCompany)
        {
            if (_companyBll.AddOrUpdateEntity(new List<Material_Type>() { materialCompany }))
            {
                return Content("OK");
            }

            return Content("Error");
        }

        public ActionResult Detail(string Material_Type_Id)
        {
            Material_Type materialCompany = _companyBll.GetEntity(m => m.Material_Type_Id == Material_Type_Id);
            return View(materialCompany);
        }

        public string Delete(string Material_Type_Id)
        {
            if (_companyBll.DeleteEntity(new List<Material_Type>()
            {
                _companyBll.GetEntity(m=>m.Material_Type_Id == Material_Type_Id)
            }))
            {
                return "OK";
            }

            return "Error";
        }
    }
}