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
        private readonly IBuyApplyBll _buyApplyBll = Container.Server<IBuyApplyBll>();
        private readonly IApplyInfoBll _applyInfoBll = Container.Server<IApplyInfoBll>();
        private readonly IBuyInComingApplyBll _buyInComingApplyBll = Container.Server<IBuyInComingApplyBll>();

        #endregion 全局变量

        // GET: Stock
        public ActionResult NeedBuy()
        {
            return View();
        }

        public string GetNeedBuy(int page, int limit)
        {
            List<Buy_Apply> buyApplies = _buyApplyBll.GetEntities(m => m.Apply_Status == 1 && m.Is_Bought == false).Skip((page - 1) * limit).Take(limit).ToList();

            var data =
                new
                {
                    code = 0,
                    msg = "",
                    total = buyApplies.Count,
                    data = buyApplies
                };
            return JsonConvert.SerializeObject(data);
        }

        public string ApplyInComing(List<Apply_Info> applyInfos)
        {
            List<BuyInComing_Apply> buyInComingApplies = new List<BuyInComing_Apply>();
            foreach (Apply_Info applyInfo in applyInfos)
            {
                BuyInComing_Apply buyInComingApply = _buyInComingApplyBll.Find(applyInfo.Apply_Id);
                buyInComingApply.Is_Bought = true;
                buyInComingApplies.Add(buyInComingApply);
            }
            return _applyInfoBll.AddEntities(applyInfos) && _buyInComingApplyBll.UpdateEntities(buyInComingApplies) ? "OK" : "Error";
        }
    }
}