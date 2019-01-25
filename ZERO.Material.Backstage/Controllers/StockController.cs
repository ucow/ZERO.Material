using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class StockController : Controller
    {
        #region 全局变量

        private static readonly UnityContainerHelper Container = new UnityContainerHelper();

        private readonly IBaseApplyBll _applyBll = Container.Server<IBaseApplyBll>();
        private readonly ITeacherBll _teacherBll = Container.Server<ITeacherBll>();

        private readonly IBaseInfoBll _infoBll = Container.Server<IBaseInfoBll>();

        #endregion 全局变量

        // GET: Stock
        public ActionResult NeedBuy()
        {
            return View();
        }

        public string GetNeedBuy(int page, int limit)
        {
            List<Material_Apply> applies = _applyBll.GetEntities(m => m.Head_Aduit == 1 && m.Needbuy_Count > 0);
            var newApplies = (from l in applies

                              group l by l.Material_Id into grouped

                              select new { Name = grouped.Key, Scores = grouped.Sum(m => m.Needbuy_Count), lastDate = grouped.Max(m => m.Start_Time) }).Skip((page - 1) * limit).Take(limit).ToList();

            ArrayList datas = new ArrayList();
            foreach (var newApply in newApplies)
            {
                Material_Info info = _infoBll.GetEntity(m => m.Material_Id == newApply.Name);

                datas.Add(new
                {
                    Material_Id = newApply.Name,
                    Material_Name = info.Material_Name,
                    Company_Name = info.Company_Name,
                    Material_Size = info.Material_Size,
                    Material_UnitWeight = info.Material_UnitWeight,
                    Needbuy_Count = newApply.Scores,
                    Material_CountUnit = info.Material_CountUnit,
                    Last_Time = newApply.lastDate
                });
            }

            var data =
                new
                {
                    code = 0,
                    msg = "",
                    total = datas.Count,
                    data = datas
                };
            return JsonConvert.SerializeObject(data);
        }
    }
}