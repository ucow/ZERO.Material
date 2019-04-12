using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
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
        #region 全局变量

        private readonly IBaseInfoBll _infoBll = UnityContainerHelper.Server<IBaseInfoBll>();
        private readonly ITypeBll _typeBll = UnityContainerHelper.Server<ITypeBll>();
        private readonly ICompanyBll _companyBll = UnityContainerHelper.Server<ICompanyBll>();
        private readonly IBaseApplyBll _applyBll = UnityContainerHelper.Server<IBaseApplyBll>();
        private readonly IApplyInfoBll _applyInfoBll = UnityContainerHelper.Server<IApplyInfoBll>();
        private readonly IBuyInComingApplyBll _buyInComingBll = UnityContainerHelper.Server<IBuyInComingApplyBll>();
        private readonly IUseApplyBll _useApplyBll = UnityContainerHelper.Server<IUseApplyBll>();
        private readonly IBaseCompanyBll _baseCompanyBll = UnityContainerHelper.Server<IBaseCompanyBll>();

        #endregion 全局变量

        [CheckLogin(IsChecked = false)]
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
        [CheckLogin(IsChecked = false)]
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
                        materialInfos.AddRange(_infoBll.GetEntities(m => m.Company_Name == company && m.Is_Show == true));
                    }
                }
                else
                {
                    List<string> typeNames = new List<string>();
                    GetChildTypes(typeNames, type);
                    materialInfos.AddRange(string.IsNullOrWhiteSpace(company)
                        ? _infoBll.GetEntities(m => typeNames.Contains(m.Material_Type_Name) && m.Is_Show == true)
                        : _infoBll.GetEntities(m =>
                            typeNames.Contains(m.Material_Type_Name) && m.Company_Name == company && m.Is_Show == true));
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(type))
                {
                    materialInfos.AddRange(string.IsNullOrWhiteSpace(company)
                        ? _infoBll.GetEntities(m => m.Material_Name.Contains(material) && m.Is_Show == true)
                        : _infoBll.GetEntities(m => m.Material_Name.Contains(material) && m.Company_Name == company && m.Is_Show == true));
                }
                else
                {
                    List<string> typeNames = new List<string>();
                    GetChildTypes(typeNames, type);
                    materialInfos.AddRange(string.IsNullOrWhiteSpace(company)
                        ? _infoBll.GetEntities(m => typeNames.Contains(type) && m.Material_Name.Contains(material) && m.Is_Show == true)
                        : _infoBll.GetEntities(m =>
                            typeNames.Contains(m.Material_Type_Name) && m.Company_Name == company &&
                            m.Material_Name.Contains(material) && m.Is_Show == true));
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
        [CheckLogin(IsChecked = false)]
        [OutputCache(Duration = Int32.MaxValue, Location = OutputCacheLocation.Client)]
        public ActionResult MaterialInfos()
        {
            List<Material_Type> materialTypes = _typeBll.GetEntities(m => true);
            Dictionary<string, List<string>> infoChildTypes = new Dictionary<string, List<string>>();
            List<string> parentTypeNames = materialTypes.Where(m => m.Material_Type_Parent_Id == "000000")
                .Select(m => m.Material_Type_Name).ToList();
            Dictionary<string, List<Material_Info>> infoDictionary = new Dictionary<string, List<Material_Info>>();

            foreach (string parentTypeName in parentTypeNames)
            {
                List<string> childNames = new List<string>();
                GetChildTypes(childNames, parentTypeName);
                infoChildTypes.Add(parentTypeName, childNames.Take(5).ToList());
            }
            ViewBag.childtypes = infoChildTypes;
            int pageIndex = 1;
            //List<string> typeNames = materialTypes.Select(m=>m.Material_Type_Name).ToList();
            while (true)
            {
                List<Material_Info> materialInfos = _infoBll.GetPageEntities(pageIndex, 200, m => m.Material_Id,(m=>true), out int total);
                foreach (Material_Info materialInfo in materialInfos)
                {
                    string parentTypeName = GetParentTypeName(materialInfo.Material_Type_Name, materialTypes);
                    if (!infoDictionary.ContainsKey(parentTypeName))
                    {
                        infoDictionary.Add(parentTypeName, new List<Material_Info>()
                        {
                            materialInfo
                        });
                    }
                    else
                    {
                        if (infoDictionary[parentTypeName].Count < 10)
                        {
                            infoDictionary[parentTypeName].Add(materialInfo);
                        }
                    }
                }

                int count = infoDictionary.Count(m => m.Value.Count >= 10);
                if (count >= parentTypeNames.Count - 2)
                {
                    break;
                }
                pageIndex++;
            }

            return View(infoDictionary);
        }

        private string GetParentTypeName(string typeName, List<Material_Type> materialTypes)
        {
            Material_Type materialType = materialTypes.FirstOrDefault(m => m.Material_Type_Name == typeName);
            int count = materialTypes.Count(m => m.Material_Type_Parent_Id == "000000");
            if (materialType?.Material_Type_Parent_Id == "000000")
            {
                return materialType.Material_Type_Name;
            }
            else
            {
                if (materialType != null)
                {
                    string name = materialTypes.FirstOrDefault(m => m.Material_Type_Id == materialType.Material_Type_Parent_Id)
                        ?.Material_Type_Name;
                    return GetParentTypeName(name, materialTypes);
                }
                else
                    return null;
            }
        }


        /// <summary>
        /// 器材详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CheckLogin(IsChecked = false)]
        public ActionResult MaterialInfo(string id)
        {
            Material_Info materialInfo = _infoBll.GetEntity(m => m.Material_Id == id && m.Is_Show == true);
            Stack<string> materType = new Stack<string>();
            materType.Push(materialInfo.Material_Type_Name);
            GetAllTypes(materType, materialInfo.Material_Type_Name);
            ViewBag.Title = materialInfo.Material_Name;
            ViewBag.AllTypes = materType;
            return View(materialInfo);
        }

        #region 我的器材

        public ActionResult Apply(string id, string count)
        {
            Dictionary<Material_Info, string> infos = new Dictionary<Material_Info, string>();
            var cookie = Request.Cookies["userInfo"];
            if (cookie != null)
            {
                string name = cookie.Value;
                var user = JsonConvert.DeserializeObject<UserInfo>(UrlHelper.DecodeUrl(name));
                var teacher = UnityContainerHelper.Server<ITeacherBll>().Find(user.username);
                ViewBag.teacher = teacher;
            }

            if (id == null && count == null)
            {
                if (Session["materialCar"] == null)
                {
                    return Content("我的器材为空");
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
            if (Session["materialCar"] != null)
            {
                if (Session["materialCar"] is Dictionary<string, int> carInfos)
                {
                    List<Material_Apply> materialApplies = new List<Material_Apply>();
                    List<BuyInComing_Apply> buyInComingApplies = new List<BuyInComing_Apply>();
                    foreach (string carInfosKey in carInfos.Keys)
                    {
                        Material_Apply newApply = (Material_Apply)materialApply.Clone();
                        newApply.Material_Id = carInfosKey;
                        materialApplies.Add(newApply);
                        var remainCount = _infoBll.GetEntity(m => m.Material_Id == carInfosKey).Material_RemainCont;
                        if (carInfos[carInfosKey] > Int32.Parse(remainCount.ToString()))
                        {
                            buyInComingApplies.Add(new BuyInComing_Apply()
                            {
                                Material_Id = carInfosKey,
                                Last_Time = materialApply.Start_Time,
                            });
                        }
                    }

                    if (_applyBll.AddEntities(materialApplies) && _buyInComingBll.AddEntities(buyInComingApplies))
                    {
                        List<Apply_Info> applyInfos = new List<Apply_Info>();
                        List<Material_Base_Company> baseCompanies = new List<Material_Base_Company>();
                        foreach (Material_Apply apply in materialApplies)
                        {
                            applyInfos.Add(new Apply_Info()
                            {
                                Apply_Status = 0,
                                Apply_Count = carInfos[apply.Material_Id],
                                ApplyType_Id = "001",
                                Apply_Id = apply.Id
                            });

                            Material_Base_Company baseCompany = _baseCompanyBll.GetEntity(m => m.Material_Id == apply.Material_Id);
                            baseCompany.Material_RemainCont =
                                baseCompany.Material_RemainCont - carInfos[apply.Material_Id] >= 0
                                    ? baseCompany.Material_RemainCont - carInfos[apply.Material_Id]
                                    : 0;
                            baseCompanies.Add(baseCompany);
                        }

                        foreach (BuyInComing_Apply buyInComingApply in buyInComingApplies)
                        {
                            var remainCount = _infoBll.GetEntity(m => m.Material_Id == buyInComingApply.Material_Id).Material_RemainCont;

                            applyInfos.Add(new Apply_Info()
                            {
                                Apply_Status = 0,
                                Apply_Count = carInfos[buyInComingApply.Material_Id] - Int32.Parse(remainCount.ToString()),
                                ApplyType_Id = "002",
                                Apply_Id = buyInComingApply.Id,
                            });
                        }

                        if (_applyInfoBll.AddEntities(applyInfos) && _baseCompanyBll.UpdateEntities(baseCompanies))
                        {
                            Session["materialCar"] = null;
                            return Content("OK");
                        }
                    }
                }
            }

            return Content("Error");
        }

        public ActionResult Success()
        {
            return View();
        }

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

        public ActionResult History()
        {
            var cookie = Request.Cookies["userInfo"];
            if (cookie != null)
            {
                string name = cookie.Value;
                var user = JsonConvert.DeserializeObject<UserInfo>(UrlHelper.DecodeUrl(name));
                string teacherName = UnityContainerHelper.Server<ITeacherBll>().Find(user.username).Teacher_Name;
                List<Use_Apply> applies = _useApplyBll.GetEntities(m => m.Teacher_Name == teacherName);
                return View(applies);
            }

            return Content("登录信息失效,请重新登录");
        }

        public ActionResult GetUserName(string username)
        {
            return Content(UnityContainerHelper.Server<ITeacherBll>().Find(username).Teacher_Name);
        }

        [CheckLogin(IsChecked = false)]
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
                string name = _typeBll.Find(materialType.Material_Type_Parent_Id).Material_Type_Name;
                materType.Push(name);
                GetAllTypes(materType, name);
            }
        }

        /// <summary>
        /// 获取父类别的子类别的集合
        /// </summary>
        /// <param name="childName">子类别名称集合</param>
        /// <param name="typeName">父类别名称</param>
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

        [CheckLogin(IsChecked = false)]
        public FileContentResult GetImage(string id)
        {
            Material_Info info = _infoBll.GetEntity(m => m.Material_Id == id && m.Is_Show == true);
            if (info != null)
            {
                return new FileContentResult(info.Material_Image, "Image/jpg");
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断服务器端是否存在该session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public bool IsHavSession(string sessionId)
        {
            return Session[sessionId] != null;
        }

        public string CancelApply(int applyId)
        {
            Material_Apply apply = _applyBll.Find(applyId);
            Apply_Info applyInfo = _applyInfoBll.Find(applyId, "001");
            if (_applyInfoBll.DeleteEntity(new List<Apply_Info>() { applyInfo }) && _applyBll.DeleteEntity(new List<Material_Apply>() { apply }))
            {
                return "OK";
            }

            return "Error";
        }
    }
}