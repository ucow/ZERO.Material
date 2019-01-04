using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class ZeroController : Controller
    {
        private static readonly UnityContainerHelper Container = new UnityContainerHelper();
        private readonly IBaseInfoBll _infoBll = Container.Server<IBaseInfoBll>();
        private readonly ITypeBll _typeBll = Container.Server<ITypeBll>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string material, int index)
        {
            ViewBag.Title = material;
            string[] filter = material.Split('_');
            List<Material_Info> materialInfos = new List<Material_Info>();
            string name = filter[0];
            if (filter.Length >= 2)
            {
                if (filter[1].Length != 0)
                {
                    string company = filter[1];
                    if (filter.Length >= 3)
                    {
                        if (filter[2].Length != 0)
                        {
                            string type = filter[2];
                            materialInfos.AddRange(_infoBll.GetEntities(m => m.Material_Name.Contains(name) && m.Company_Name == company && m.Material_Type_Name == type).Distinct().ToList());
                        }
                    }
                    else
                    {
                        materialInfos.AddRange(_infoBll.GetEntities(m => m.Material_Name.Contains(name) && m.Company_Name == company).Distinct().ToList());
                    }
                }
            }
            else
            {
                materialInfos.AddRange(_infoBll.GetEntities(m => m.Material_Name.Contains(name)).Distinct().ToList());
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
    }
}