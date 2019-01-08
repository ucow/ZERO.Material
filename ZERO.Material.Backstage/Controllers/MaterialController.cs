﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
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
        private readonly IBaseInfoBll _baseInfoBll = Container.Server<IBaseInfoBll>();
        private readonly IBaseTypeBll _baseTypeBll = Container.Server<IBaseTypeBll>();
        private readonly IBaseCompanyBll _baseCompanyBll = Container.Server<IBaseCompanyBll>();

        // GET: Material
        public ActionResult Index()
        {
            return View();
        }

        public string GetModelInfo()
        {
            return AssmblyHelper.GetDisplayAttributeInfo<Material_Info>();
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
            Material_Info materialInfo = _baseInfoBll.GetEntity(m => m.Material_Id == material_Id);
            if (materialInfo != null)
            {
                return View(materialInfo);
            }
            return View();
        }

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

        public string List(int page, int limit)
        {
            List<Material_Info> materialInfos = _baseInfoBll.GetPageEntities(page, limit, m => m.Material_Id, out int total);
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
                _baseBll.GetEntity(m=>m.Material_Id == material_Id)
            }))
            {
                return "OK";
            }

            return "Error";
        }
    }
}