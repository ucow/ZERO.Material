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
        private readonly IBuyApplyBll _buyApplyBll = Container.Server<IBuyApplyBll>();
        private readonly IApplyInfoBll _applyInfoBll = Container.Server<IApplyInfoBll>();
        private readonly IBuyInComingApplyBll _buyInComingApplyBll = Container.Server<IBuyInComingApplyBll>();
        private readonly IBaseCompanyBll _baseCompanyBll = Container.Server<IBaseCompanyBll>();
        private readonly IUseApplyBll _useApplyBll = Container.Server<IUseApplyBll>();
        private readonly IBaseApplyBll _baseApplyBll = Container.Server<IBaseApplyBll>();

        #endregion 全局变量

        // GET: Stock

        #region 需采购

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

        #endregion 需采购

        #region 可入库

        public ActionResult CanInComing()
        {
            return View();
        }

        public string CanInComingBuy()
        {
            //入库申请 被批准 没有入库
            List<Buy_Apply> buyApplies = _buyApplyBll.GetEntities(m => m.ApplyType_Id == "003" && m.Apply_Status == 1 && m.Is_InComed == false);
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

        public string ConfirmInComing(List<Apply_Info> applyInfos)
        {
            List<BuyInComing_Apply> buyInComingApplies = new List<BuyInComing_Apply>();
            List<Material_Base_Company> baseCompanies = new List<Material_Base_Company>();
            foreach (var applyInfo in applyInfos)
            {
                BuyInComing_Apply buyInComingApply = _buyInComingApplyBll.GetEntity(m => m.Id == applyInfo.Apply_Id);
                buyInComingApply.Is_InComed = true;
                Material_Base_Company materialBaseCompany =
                    _baseCompanyBll.GetEntity(m => m.Material_Id == buyInComingApply.Material_Id);
                materialBaseCompany.Material_RemainCont += applyInfo.Apply_Count;
                if (materialBaseCompany.Material_RemainCont > materialBaseCompany.Material_Count)
                    materialBaseCompany.Material_Count = materialBaseCompany.Material_RemainCont;
                baseCompanies.Add(materialBaseCompany);
                buyInComingApplies.Add(buyInComingApply);
            }

            return _buyInComingApplyBll.UpdateEntities(buyInComingApplies) &&
                   _baseCompanyBll.UpdateEntities(baseCompanies)
                ? "OK"
                : "Error";
        }

        #endregion 可入库

        #region 可出库

        public ActionResult CanOutComing()
        {
            return View();
        }

        public string GetCanOutComing(int page, int limit)
        {
            List<Use_Apply> useApplies = _useApplyBll.GetEntities(m => m.Apply_Status == 1 && m.Is_Get == false);

            var data =
                new
                {
                    code = 0,
                    msg = "",
                    total = useApplies.Count,
                    data = useApplies
                };
            return JsonConvert.SerializeObject(data);
        }

        public string ConfirmOutComing(List<Apply_Info> applyInfos)
        {
            List<Material_Apply> materialApplies = new List<Material_Apply>();
            foreach (Apply_Info applyInfo in applyInfos)
            {
                Material_Apply materialApply = _baseApplyBll.GetEntity(m => m.Id == applyInfo.Apply_Id);
                materialApply.Is_Get = true;
                materialApplies.Add(materialApply);
            }

            return _baseApplyBll.UpdateEntities(materialApplies) ? "OK" : "Error";
        }

        #endregion 可出库

        #region 申请入库

        public ActionResult ApplyInComing()
        {
            return View();
        }

        #endregion 申请入库
    }
}