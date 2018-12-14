using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class MaterialController : Controller
    {
        private static readonly UnityContainerHelper Container = new UnityContainerHelper();
        private readonly IBaseBll _baseBll = Container.Server<IBaseBll>();
        private readonly ITypeBll _typeBll = Container.Server<ITypeBll>();
        private readonly ICompanyBll _companyBll = Container.Server<ICompanyBll>();

        // GET: Material
        public ActionResult Index()
        {
            return View();
        }

        public string GetModelInfo()
        {
            return AssmblyHelper.GetDisplayAttributeInfo<Material_Base>();
        }

        public List<Material_Type> GetTypeList()
        {
            return _typeBll.GetEntities(p => true);
        }

        //更新或添加数据
        public ActionResult Add(string material_Id)
        {
            ViewBag.types = _typeBll.GetEntities(p => true).Select(p => p.Material_Type_Name);
            ViewBag.companys = _companyBll.GetEntities(p => true).Select(p => p.Company_Name);
            if (string.IsNullOrEmpty(material_Id))
            {
                ViewBag.IsUpdate = false;
                return View();
            }

            ViewBag.IsUpdate = true;
            Material_Base materialMessage = _baseBll.GetEntity(m => m.Material_Id == material_Id);
            if (materialMessage != null)
            {
                materialMessage.Type_Id = _typeBll.GetEntity(m => m.Material_Type_Id == materialMessage.Type_Id)
                    .Material_Type_Name;
                materialMessage.Company_Id =
                    _companyBll.GetEntity(m => m.Company_Id == materialMessage.Company_Id).Company_Name;
                return View(materialMessage);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add(Material_Base materialBase)
        {
            materialBase.Company_Id = _companyBll.GetEntity(m => m.Company_Name == materialBase.Company_Id).Company_Id;
            materialBase.Type_Id =
                _typeBll.GetEntity(m => m.Material_Type_Name == materialBase.Type_Id).Material_Type_Id;
            if (_baseBll.AddOrUpdateEntity(new List<Material_Base>() { materialBase }))
            {
                return Content("OK");
            }

            return Content("Error");
        }

        public string List(int page, int limit)
        {
            List<Material_Base> materialMessages = _baseBll.GetPageEntities(page, limit, (m => m.Material_Id), out var total);
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

        public ActionResult Detail(string material_Id)
        {
            Material_Base materialMessage = _baseBll.GetEntity(m => m.Material_Id == material_Id);
            return View(materialMessage);
        }

        public string Delete(string material_Id)
        {
            if (_baseBll.DeleteEntity(new List<Material_Base>()
            {
                _baseBll.GetEntity(m=>m.Material_Id == material_Id)
            }))
            {
                return "OK";
            }

            return "Error";
        }
    }
}