using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class MaterialController : Controller
    {
        private readonly IBaseBll _baseBll = new UnityContainerHelper().Server<IBaseBll>();

        // GET: Material
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(string materialId)
        {
            if (string.IsNullOrEmpty(materialId))
            {
                return View();
            }

            Material_Base materialBase = _baseBll.GetEntity(m => m.Material_Id == materialId);
            return View(materialBase);
        }

        [HttpPost]
        public ActionResult Add(Material_Base materialBase)
        {
            if (_baseBll.AddOrUpdateEntity(new List<Material_Base>() { materialBase }))
            {
                return Content("OK");
            }

            return Content("Error");
        }

        public string List(int page, int limit)
        {
            List<Material_Base> materialBases = _baseBll.GetPageEntities(page, limit, (m => m.Material_Id), out var total);
            var dataJson = new
            {
                code = 0,
                msg = "OK",
                count = total,
                data = materialBases
            };
            string json = JsonConvert.SerializeObject(dataJson);
            return json;
        }

        public ActionResult Detail(string materialId)
        {
            Material_Base materialBase = _baseBll.GetEntity(m => m.Material_Id == materialId);
            return View(materialBase);
        }

        public string Delete(string materialId)
        {
            if (_baseBll.DeleteEntity(new List<Material_Base>()
            {
                _baseBll.GetEntity(m=>m.Material_Id == materialId)
            }))
            {
                return "OK";
            }

            return "Error";
        }

        public ActionResult Update(string materialId)
        {
            Material_Base materialBase = _baseBll.GetEntity(m => m.Material_Id == materialId);
            return View(materialBase);
        }
    }
}