using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class ZeroController : Controller
    {
        private static readonly UnityContainerHelper Container = new UnityContainerHelper();
        private IBaseInfoBll _infoBll = Container.Server<IBaseInfoBll>();
        private ITypeBll _typeBll = Container.Server<ITypeBll>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string material, int index)
        {
            List<Material_Info> materialInfos = _infoBll.GetEntities(m => m.Material_Name.Contains(material)).Distinct().ToList();
            List<Material_Info> infos = materialInfos.Skip(index * 12).Take(12).ToList();
            List<string> sizes = new List<string>();
            List<string> names = materialInfos.Select(m => m.Material_Name).Distinct().ToList();
            foreach (string name in names)
            {
                sizes.Add(name.Split('_')[1]);
            }

            ViewBag.sizes = sizes;
            ViewBag.companies = materialInfos.Select(m => m.Company_Name).Distinct().ToList();
            ViewBag.types = materialInfos.Select(m => m.Material_Type_Name).Distinct().ToList();
            ViewBag.total = materialInfos.Count % 12 == 0 ? materialInfos.Count / 12 - 1 : materialInfos.Count / 12;
            ViewBag.material = material;
            ViewBag.index = index;
            return View(infos);
        }

        public ActionResult MaterialInfos()
        {
            List<string> types = _typeBll.GetEntities(m => true).Select(m => m.Material_Type_Name).Distinct().ToList();
            Dictionary<string, List<Material_Info>> infoDictionary = new Dictionary<string, List<Material_Info>>();
            foreach (string type in types)
            {
                infoDictionary.Add(type, _infoBll.GetEntities(m => m.Material_Type_Name == type).Distinct().Take(10).ToList());
            }

            return View(infoDictionary);
        }

        public ActionResult MaterialInfo(string id)
        {
            Material_Info materialInfo = _infoBll.GetEntity(m => m.Material_Id == id);
            return View(materialInfo);
        }
    }
}