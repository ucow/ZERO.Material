using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class TypeController : BaseController<Material_Type>
    {
        private ITypeBll _typeBll = UnityContainerHelper.Server<ITypeBll>();

        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public string List()
        {
            List<Material_Type> materialTypes = new List<Material_Type>();
            List<Material_Type> parenTypes = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == "000000");
            foreach (Material_Type parenType in parenTypes)
            {
                parenType.Material_Type_Parent_Id = "-1";
                materialTypes.Add(parenType);
                List<Material_Type> child =
                    _typeBll.GetEntities(m => m.Material_Type_Parent_Id == parenType.Material_Type_Id);
                foreach (Material_Type type in child)
                {
                    materialTypes.Add(type);
                    List<Material_Type> childTypes =
                        _typeBll.GetEntities(m => m.Material_Type_Parent_Id == type.Material_Type_Id);
                    foreach (Material_Type childType in childTypes)
                    {
                        materialTypes.Add(childType);
                    }
                }
            }

            var dataJson = new
            {
                code = 0,
                msg = "OK",
                count = materialTypes.Count,
                data = materialTypes
            };
            string json = JsonConvert.SerializeObject(dataJson);
            return json;
        }

        public ActionResult Add(string Material_Type_Id)
        {
            List<Material_Type> topTypes = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == "000000");
            List<string> topIds = topTypes.Select(n => n.Material_Type_Id).ToList();
            List<Material_Type> centerTypes = _typeBll.GetEntities(m =>
                topIds.Contains(m.Material_Type_Parent_Id));
            ViewBag.topTypes = topTypes;
            ViewBag.centerTypes = centerTypes;

            if (string.IsNullOrEmpty(Material_Type_Id))
            {
                ViewBag.IsUpdate = false;
                return View();
            }
            ViewBag.IsUpdate = true;
            Material_Type materialType = _typeBll.GetEntity(m => m.Material_Type_Id == Material_Type_Id);

            if (materialType == null)
                return View();
            ViewBag.parent = materialType.Material_Type_Parent_Id == "000000" ? "无" : _typeBll.GetEntity(m => m.Material_Type_Id == materialType.Material_Type_Parent_Id)
                .Material_Type_Name;
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
                type.Material_Type_Parent_Id = materialType.Material_Type_Parent_Id;
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
            ViewBag.parent = materialType.Material_Type_Parent_Id == "000000" ? "无" : _typeBll.GetEntity(m => m.Material_Type_Id == materialType.Material_Type_Parent_Id)
                  .Material_Type_Name;
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