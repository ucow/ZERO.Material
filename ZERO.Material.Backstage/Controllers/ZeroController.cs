using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZERO.Material.Backstage.Filter;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;
using ZERO.Material.Model.Other;
using UrlHelper = ZERO.Material.Command.UrlHelper;

namespace ZERO.Material.Backstage.Controllers
{
    public class ZeroController : Controller
    {
        private static readonly UnityContainerHelper Container = new UnityContainerHelper();
        private readonly IBaseInfoBll _infoBll = Container.Server<IBaseInfoBll>();
        private readonly ITypeBll _typeBll = Container.Server<ITypeBll>();
        private ICompanyBll _companyBll = Container.Server<ICompanyBll>();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="material"></param>
        /// <param name="type"></param>
        /// <param name="company"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ActionResult Search(string material, string type, string company, int index)
        {
            ViewBag.Title = $"{material}_{company}_{type}";

            ViewBag.type = type;
            ViewBag.company = company;
            List<Material_Info> materialInfos = new List<Material_Info>();
            if (string.IsNullOrWhiteSpace(material))
            {
                if (string.IsNullOrWhiteSpace(type))
                {
                    if (string.IsNullOrWhiteSpace(company))
                    {
                        return View();
                    }
                    else
                    {
                        materialInfos.AddRange(_infoBll.GetEntities(m => m.Company_Name == company));
                    }
                }
                else
                {
                    List<string> typeNames = new List<string>();
                    GetChildTypes(typeNames, type);
                    materialInfos.AddRange(string.IsNullOrWhiteSpace(company)
                        ? _infoBll.GetEntities(m => typeNames.Contains(m.Material_Type_Name))
                        : _infoBll.GetEntities(m =>
                            typeNames.Contains(m.Material_Type_Name) && m.Company_Name == company));
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(type))
                {
                    materialInfos.AddRange(string.IsNullOrWhiteSpace(company)
                        ? _infoBll.GetEntities(m => m.Material_Name.Contains(material))
                        : _infoBll.GetEntities(m => m.Material_Name.Contains(material) && m.Company_Name == company));
                }
                else
                {
                    List<string> typeNames = new List<string>();
                    GetChildTypes(typeNames, type);
                    materialInfos.AddRange(string.IsNullOrWhiteSpace(company)
                        ? _infoBll.GetEntities(m => typeNames.Contains(type) && m.Material_Name.Contains(material))
                        : _infoBll.GetEntities(m =>
                            typeNames.Contains(m.Material_Type_Name) && m.Company_Name == company &&
                            m.Material_Name.Contains(material)));
                }
            }

            //            List<Material_Info> materialInfos = _infoBll.GetEntities(m => m.Material_Name.Contains(material)).Distinct().ToList();

            if (materialInfos.Count == 0)
            {
                return View();
            }
            List<Material_Info> infos = materialInfos.Skip(index * 12).Take(12).ToList();
            //List<string> sizes = new List<string>();

            #region 规格

            //List<string> names = materialInfos.Select(m => m.Material_Size).Distinct().ToList();
            //foreach (string name in names)
            //{
            //    sizes.Add(name);
            //}

            //ViewBag.sizes = sizes;

            #endregion 规格

            ViewBag.companies = materialInfos.Select(m => m.Company_Name).Distinct().ToList();
            ViewBag.types = materialInfos.Select(m => m.Material_Type_Name).Distinct().ToList();
            ViewBag.total = materialInfos.Count % 12 == 0 ? materialInfos.Count / 12 - 1 : materialInfos.Count / 12;

            ViewBag.index = index;
            return View(infos);
        }

        /// <summary>
        /// 器材列表
        /// </summary>
        /// <returns></returns>
        public ActionResult MaterialInfos()
        {
            Dictionary<string, List<Material_Info>> infoDictionary = new Dictionary<string, List<Material_Info>>();
            Dictionary<string, List<string>> infoChildTypes = new Dictionary<string, List<string>>();
            List<Material_Type> materialTypes = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == "000000");
            List<Material_Info> materialInfos = new List<Material_Info>();
            if (materialTypes.Count > 0)
            {
                foreach (Material_Type materialType in materialTypes)
                {
                    infoChildTypes.Add(materialType.Material_Type_Name, _typeBll.GetEntities(m => m.Material_Type_Parent_Id == materialType.Material_Type_Id).Select(m => m.Material_Type_Name).Take(5).ToList());

                    List<string> materialIds = _typeBll
                        .GetEntities(m => m.Material_Type_Parent_Id == materialType.Material_Type_Id)
                        .Select(m => m.Material_Type_Id).ToList();
                    if (materialIds.Count > 0)
                    {
                        foreach (string materialId in materialIds)
                        {
                            List<string> typeNames = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == materialId)
                                .Select(m => m.Material_Type_Name).ToList();
                            if (typeNames.Count > 0)
                            {
                                foreach (string typeName in typeNames)
                                {
                                    materialInfos.AddRange(_infoBll.GetEntities(m => m.Material_Type_Name == typeName));

                                    if (materialInfos.Count >= 10)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    infoDictionary.Add(materialType.Material_Type_Name, materialInfos.Take(10).ToList());
                    materialInfos.Clear();
                }
            }

            ViewBag.childtypes = infoChildTypes;
            return View(infoDictionary);
        }

        /// <summary>
        /// 器材详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MaterialInfo(string id)
        {
            Material_Info materialInfo = _infoBll.GetEntity(m => m.Material_Id == id);
            Stack<string> materType = new Stack<string>();
            materType.Push(materialInfo.Material_Type_Name);
            GetAllTypes(materType, materialInfo.Material_Type_Name);
            ViewBag.Title = materialInfo.Material_Name;
            ViewBag.AllTypes = materType;
            return View(materialInfo);
        }

        #region 我的器材

        [CheckLogin]
        public ActionResult Apply(string id, string count)
        {
            Dictionary<Material_Info, string> infos = new Dictionary<Material_Info, string>();
            var cookie = Request.Cookies["userInfo"];
            if (cookie != null)
            {
                string name = cookie.Value;
                var user = JsonConvert.DeserializeObject<UserInfo>(UrlHelper.DecodeUrl(name));
                var teacher = Container.Server<ITeacherBll>().GetEntity(m => m.Teacher_Id == user.username);
                ViewBag.teacher = teacher;
            }

            if (id == null && count == null)
            {
                if (Session["materialCar"] == null)
                {
                    return Content("购物车为空");
                }

                if (Session["materialCar"] is Dictionary<string, int> carInfos)
                {
                    foreach (string carInfosKey in carInfos.Keys)
                    {
                        Material_Info info = _infoBll.GetEntity(m => m.Material_Id == carInfosKey);
                        int buyCount = carInfos[carInfosKey] > Int32.Parse(info.Material_RemainCont.ToString())
                            ? carInfos[carInfosKey] - Int32.Parse(info.Material_RemainCont.ToString())
                            : 0;
                        string counts = carInfos[carInfosKey] + ":" + buyCount;
                        infos.Add(info, counts);
                    }
                }
            }
            else
            {
                Material_Info materialInfo = _infoBll.GetEntity(m => m.Material_Id == id);
                ViewBag.materialInfo = materialInfo;
                var buyCount = Int32.Parse(count) > Int32.Parse(materialInfo.Material_RemainCont.ToString())
                    ? Int32.Parse(count) - Int32.Parse(materialInfo.Material_RemainCont.ToString())
                    : 0;
                infos.Add(materialInfo, count + ":" + buyCount);
                if (Session["materialCar"] == null)
                {
                    var carInfos = new Dictionary<string, int> { { id, int.Parse(count) } };
                    Session["materialCar"] = carInfos;
                }
                else
                {
                    if (Session["materialCar"] is Dictionary<string, int> carInfos)
                    {
                        if (carInfos.ContainsKey(id))
                        {
                            carInfos[id] += int.Parse(count);
                        }
                        else
                        {
                            carInfos.Add(id, int.Parse(count));
                        }
                        Session["materialCar"] = carInfos;
                    }
                }
            }
            ViewBag.materialInfos = infos;
            var model = new Material_Apply();
            return View(model);
        }

        [HttpPost]
        public ActionResult Apply(Material_Apply materialApply)
        {
            IBaseApplyBll applyBll = Container.Server<IBaseApplyBll>();
            materialApply.Head_Aduit = 0;
            List<Material_Apply> materialApplies = new List<Material_Apply>();
            if (Session["materialCar"] != null)
            {
                if (Session["materialCar"] is Dictionary<string, int> carInfos)
                {
                    foreach (string carInfosKey in carInfos.Keys)
                    {
                        Material_Apply newApply = (Material_Apply)materialApply.Clone();
                        var remainCount = _infoBll.GetEntity(m => m.Material_Id == carInfosKey).Material_RemainCont;
                        newApply.Material_Id = carInfosKey;
                        newApply.Apply_Count = carInfos[carInfosKey];
                        newApply.Needbuy_Count = carInfos[carInfosKey] > Int32.Parse(remainCount.ToString())
                            ? carInfos[carInfosKey] - Int32.Parse(remainCount.ToString())
                            : 0;
                        materialApplies.Add(newApply);
                    }
                }
            }

            if (applyBll.AddEntities(materialApplies))
            {
                return Content("OK");
            }

            return Content("Error");
        }

        public ActionResult Success()
        {
            return View();
        }

        [CheckLogin]
        [HttpPost]
        public ActionResult MaterialCar(string material, string count)
        {
            if (Session["materialCar"] == null)
            {
                var carInfos = new Dictionary<string, int> { { material, int.Parse(count) } };
                Session["materialCar"] = carInfos;
            }
            else
            {
                if (Session["materialCar"] is Dictionary<string, int> carInfos)
                {
                    if (carInfos.ContainsKey(material))
                    {
                        carInfos[material] += int.Parse(count);
                    }
                    else
                    {
                        carInfos.Add(material, int.Parse(count));
                    }
                    Session["materialCar"] = carInfos;
                }
            }
            return Content("添加成功");
        }

        #endregion 我的器材

        public ActionResult GetUserName(string username)
        {
            return Content(Container.Server<ITeacherBll>().GetEntity(m => m.Teacher_Id == username).Teacher_Name);
        }

        public ActionResult Company(int index)
        {
            List<Material_Company> companies = _companyBll.GetEntities(m => true);
            ViewBag.index = index;
            ViewBag.total = companies.Count % 12 == 0 ? companies.Count / 12 - 1 : companies.Count / 12;
            return View(companies.OrderBy(m => m.Id).Skip((index - 1) * 12).Take(12).ToList());
        }

        private void GetAllTypes(Stack<string> materType, string typeName)
        {
            Material_Type materialType = _typeBll.GetEntity(m => m.Material_Type_Name == typeName);
            if (materialType.Material_Type_Parent_Id != "000000")
            {
                string name = _typeBll.GetEntity(m => m.Material_Type_Id == materialType.Material_Type_Parent_Id).Material_Type_Name;
                materType.Push(name);
                GetAllTypes(materType, name);
            }
        }

        private void GetChildTypes(List<string> childName, string typeName)
        {
            string typeId = _typeBll.GetEntity(m => m.Material_Type_Name == typeName).Material_Type_Id;
            List<string> typeNames = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == typeId)
                .Select(m => m.Material_Type_Name).ToList();
            if (typeNames.Count == 0)
            {
                childName.Add(typeName);
            }
            else
            {
                foreach (string name in typeNames)
                {
                    GetChildTypes(childName, name);
                }
            }
        }

        public FileContentResult GetImage(string id)
        {
            Material_Info info = _infoBll.GetEntity(m => m.Material_Id == id);
            if (info != null)
            {
                return new FileContentResult(info.Material_Image, "Image/jpg");
            }
            else
            {
                return null;
            }
        }
    }
}