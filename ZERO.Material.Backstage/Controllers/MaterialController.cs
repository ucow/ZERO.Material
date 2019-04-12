using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;
using ExpressionHelper = ZERO.Material.Command.ExpressionHelper;

namespace ZERO.Material.Backstage.Controllers
{
    public class MaterialController : BaseController<Material_Info>
    {
        private readonly IBaseBll _baseBll = UnityContainerHelper.Server<IBaseBll>();
        private readonly ITypeBll _typeBll = UnityContainerHelper.Server<ITypeBll>();
        private readonly ICompanyBll _companyBll = UnityContainerHelper.Server<ICompanyBll>();

        private readonly IBaseInfoBll _baseInfoBll = UnityContainerHelper.Server<IBaseInfoBll>();

        // GET: Material
        public ActionResult Index()
        {
            ViewBag.types = _typeBll.GetEntities(p => p.Material_Type_Parent_Id == "000000");
            ViewBag.companys = _companyBll.GetEntities(m => true).Select(m => m.Company_Name).ToList();
            return View();
        }

        public List<Material_Type> GetTypeList()
        {
            return _typeBll.GetEntities(p => true);
        }

        //更新或添加数据
        public ActionResult Add(string material_Id)
        {
            ViewBag.types = _typeBll.GetEntities(p => p.Material_Type_Parent_Id == "000000");
            ViewBag.companys = _companyBll.GetEntities(p => true).Select(p => p.Company_Name);
            if (string.IsNullOrEmpty(material_Id))
            {
                ViewBag.IsUpdate = false;
                return View();
            }

            ViewBag.IsUpdate = true;
            Material_Info materialInfo = _baseInfoBll.GetEntity(m => m.Material_Id == material_Id);
            Material_Type currentType =
                _typeBll.GetEntity(m => m.Material_Type_Name == materialInfo.Material_Type_Name);
            ViewBag.brotherTypes =
                _typeBll.GetEntities(m => m.Material_Type_Parent_Id == currentType.Material_Type_Parent_Id);
            Material_Type parenType =
                _typeBll.Find(currentType.Material_Type_Parent_Id);
            ViewBag.parentTypes =
                _typeBll.GetEntities(m => m.Material_Type_Parent_Id == parenType.Material_Type_Parent_Id);
            ViewBag.parentName = parenType.Material_Type_Name;
            ViewBag.grandName = _typeBll.Find(parenType.Material_Type_Parent_Id)
                .Material_Type_Name;
            if (materialInfo != null)
            {
                return View(materialInfo);
            }
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(Material_Info materialInfo, bool isUpdate, string oldCompanyName, string oldTypeName)
        {
            if (isUpdate)
            {
                return Content(_baseInfoBll.UpdateBaseInfo(materialInfo, oldTypeName, oldCompanyName));
            }
            else
            {
                return Content(_baseInfoBll.AddBaseInfo(materialInfo));
            }
        }

        public string List(int page, int limit, string typeId,string companyName,string materialName)
        {
            Expression<Func<Material_Info, bool>> whereLambda;
            if (string.IsNullOrWhiteSpace(typeId))
            {
                whereLambda = m => true;
            }
            else
            {
                string typeName = _typeBll.Find(typeId).Material_Type_Name;
                whereLambda = m => m.Material_Type_Name == typeName;
            }

            whereLambda = string.IsNullOrWhiteSpace(companyName) ? whereLambda.And(m=>true) : whereLambda.And(m => m.Company_Name == companyName);

            whereLambda = string.IsNullOrWhiteSpace(materialName) ? whereLambda.And(m => true) : whereLambda.And(m => m.Material_Name.Contains(materialName));

            List<Material_Info> materialInfos = _baseInfoBll.GetPageEntities(page, limit, m => m.Material_Id, whereLambda, out int total);
            var dataJson = new
            {
                code = 0,
                msg = "OK",
                count = total,
                data = materialInfos
            };
            string json = JsonConvert.SerializeObject(dataJson);
            return json;
        }

        public ActionResult Detail(string material_Id)
        {
            Material_Info materialInfo = _baseInfoBll.GetEntity(m => m.Material_Id == material_Id);
            return View(materialInfo);
        }

        public string Delete(string material_Id)
        {
            if (_baseBll.DeleteEntity(new List<Material_Base>()
            {
                _baseBll.Find(material_Id)
            }))
            {
                return "OK";
            }

            return "Error";
        }

        [HttpPost]
        public string GetChildType(string typeId)
        {
            List<Material_Type> types = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == typeId);
            if (types != null)
            {
                return JsonConvert.SerializeObject(types);
            }
            return "";
        }

        public FileContentResult GetImage(string id)
        {
            Material_Info info = _baseInfoBll.GetEntity(m => m.Material_Id == id);
            if (info != null)
            {
                return new FileContentResult(info.Material_Image, "Image/jpg");
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public string SetMaterialShow(string id, bool check)
        {
            Material_Base materialBase = _baseBll.Find(id);
            materialBase.Is_Show = check;
            if (!_baseBll.UpdateEntities(new List<Material_Base> { materialBase }))
            {
                return "Error";
            }

            return "OK";
        }
    }
}