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
        private readonly IMessageBll _messageBll = new UnityContainerHelper().Server<IMessageBll>();

        // GET: Material
        public ActionResult Index()
        {
            return View();
        }

        public string GetModelInfo()
        {
            return AssmblyHelper.GetDisplayAttributeInfo<Material_Message>();
        }

        //更新或添加数据
        public ActionResult Add(string material_Id)
        {
            if (string.IsNullOrEmpty(material_Id))
            {
                return View();
            }

            Material_Message materialMessage = _messageBll.GetEntity(m => m.Material_Id == material_Id);
            if (materialMessage == null)
                return View();
            return View(materialMessage);
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
            List<Material_Message> materialMessages = _messageBll.GetPageEntities(page, limit, (m => m.Material_Id), out var total);
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

        public ActionResult Detail(string materialId)
        {
            Material_Message materialMessage = _messageBll.GetEntity(m => m.Material_Id == materialId);
            return View(materialMessage);
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
    }
}