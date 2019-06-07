using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;
using ZERO.Material.Model.Other;

namespace ZERO.Material.Backstage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBaseInfoBll _baseInfoBll = UnityContainerHelper.Server<IBaseInfoBll>();

        public ActionResult Index()
        {
            List<int> warningCount = _baseInfoBll.ExecuteSqlCommand<int>(
                "  select count(Material_Name) FROM [ZERO_Material].[dbo].[Material_Info] where Material_Count!=0 and (Material_RemainCont/Material_Count)<=0.15");
            ViewBag.isWarning = warningCount[0] > 0;
            return View();
        }

        public ActionResult Default()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string MaterialWarning(int page, int limit)
        {
            //List<MaterialWarningModel> materialWarnings = _baseInfoBll.ExecuteSqlCommand<MaterialWarningModel>(
            //     "  Select Material_Name as MaterialName,(STR(Material_Count)+Material_CountUnit) as MaterialCount ,(STR(Material_RemainCont)+Material_CountUnit) as MaterialRemainCount ,(STR(Material_Count-Material_RemainCont)+Material_CountUnit) as MaterialBuyCount  FROM [ZERO_Material].[dbo].[Material_Info] where Material_Count!=0 and (Material_RemainCont/Material_Count)<=0.15");
            List<Material_Info> materialBases = _baseInfoBll.GetPageEntities(page, limit,
                m => m.Material_Count - m.Material_RemainCont,
                m => m.Material_Count > 0 && m.Material_RemainCont / m.Material_Count < 0.15, out int total, false);

            List<MaterialWarningModel> warningModels = new List<MaterialWarningModel>();
            materialBases.ForEach(item =>
            {
                warningModels.Add(new MaterialWarningModel
                {
                    MaterialName = item.Material_Name,
                    MaterialCount = item.Material_Count.ToString(CultureInfo.InvariantCulture),
                    MaterialRemainCount = item.Material_RemainCont.ToString(CultureInfo.InvariantCulture),
                    MaterialBuyCount = (item.Material_Count - item.Material_RemainCont).ToString(CultureInfo.InvariantCulture)
                });
            });

            var msg = new
            {
                code = 0,
                msg = "",
                count = total,
                data = warningModels
            };

            return JsonConvert.SerializeObject(msg);
        }
    }
}