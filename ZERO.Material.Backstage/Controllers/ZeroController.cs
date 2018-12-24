using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private IBaseInfoBll _infoBll = Container.Server<IBaseInfoBll>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string material, int index)
        {
            List<Material_Info> materialInfos = _infoBll.GetEntities(m => m.Material_Name.Contains(material));
            List<Material_Info> infos = materialInfos.Skip((index - 1) * 18).Skip(18).ToList();

            ViewBag.total = materialInfos.Count / 18 + 1;
            ViewBag.material = material;
            ViewBag.index = index;
            return View(infos);
        }

        public ActionResult GetImageByBytes(byte[] buffer)
        {
            MemoryStream memory = new MemoryStream(buffer);
            return File(memory.ToArray(), "image/jpeg");
        }
    }
}