using System.Collections.Generic;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class TypeController : BaseController<Material_Type>
    {
        private ITypeBll _typeBll = new UnityContainerHelper().Server<ITypeBll>();

        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public string List(int page, int limit)
        {
            List<Material_Type> materialTypes = _typeBll.GetPageEntities(page, limit, (m => m.Material_Type_Id), out var total);
            var dataJson = new
            {
                code = 0,
                msg = "OK",
                count = total,
                data = materialTypes
            };
            string json = JsonConvert.SerializeObject(dataJson);
            return json;
        }

        public ActionResult Add(string Material_Type_Id)
        {
            if (string.IsNullOrEmpty(Material_Type_Id))
            {
                ViewBag.IsUpdate = false;
                return View();
            }
            ViewBag.IsUpdate = true;
            Material_Type materialType = _typeBll.GetEntity(m => m.Material_Type_Id == Material_Type_Id);
            if (materialType == null)
                return View();
            return View(materialType);
        }

        [HttpPost]
        public ActionResult Add(Material_Type materialType, bool isUpdate)
        {
            if (isUpdate)
            {
                Material_Type type = _typeBll.Find(materialType.Material_Type_Id);
                type.Material_Type_Name = materialType.Material_Type_Name;
                type.Material_Type_Remark = materialType.Material_Type_Remark;

                return _typeBll.UpdateEntities(new List<Material_Type>() { type }) ? Content("OK") : Content("Error");
            }
            else
            {
                return _typeBll.AddEntities(new List<Material_Type>() { materialType }) ? Content("OK") : Content("Error");
            }
        }

        public ActionResult Detail(string Material_Type_Id)
        {
            Material_Type materialType = _typeBll.GetEntity(m => m.Material_Type_Id == Material_Type_Id);
            return View(materialType);
        }

        public string Delete(string Material_Type_Id)
        {
            if (_typeBll.DeleteEntity(new List<Material_Type>()
            {
                _typeBll.GetEntity(m=>m.Material_Type_Id == Material_Type_Id)
            }))
            {
                return "OK";
            }

            return "Error";
        }
    }
}