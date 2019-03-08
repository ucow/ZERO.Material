using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class ChartController : Controller
    {
        #region 全局变量

        private readonly ITypeBll _typeBll = UnityContainerHelper.Server<ITypeBll>();
        private readonly IBaseTypeBll _baseTypeBll = UnityContainerHelper.Server<IBaseTypeBll>();

        #endregion 全局变量

        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetTypeChartData()
        {
            List<Material_Type> materialTypes = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == "000000");

            List<string> xAxisData = materialTypes.Select(m => m.Material_Type_Name).ToList();
            List<int> yAxisData = new List<int>();
            foreach (Material_Type materialType in materialTypes)
            {
                yAxisData.Add(GetTypeNumber(materialType.Material_Type_Id));
            }

            var dataJson = new
            {
                xAxis = xAxisData,
                yAxis = yAxisData
            };

            return JsonConvert.SerializeObject(dataJson);
        }

        #region 私有方法

        private int GetTypeNumber(string typeId)
        {
            int count = 0;
            List<Material_Type> materialTypes = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == typeId);
            if (materialTypes.Count == 0)
            {
                return count + _baseTypeBll.GetEntities(m => m.Material_Type_Id == typeId).Count;
            }

            foreach (Material_Type materialType in materialTypes)
            {
                count += GetTypeNumber(materialType.Material_Type_Id);
            }

            return count;
        }

        #endregion 私有方法
    }
}