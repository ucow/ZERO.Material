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
    public class StockController : BaseController<Apply_Info>
    {
        #region 全局变量

        private readonly IBuyApplyBll _buyApplyBll = UnityContainerHelper.Server<IBuyApplyBll>();
        private readonly IApplyInfoBll _applyInfoBll = UnityContainerHelper.Server<IApplyInfoBll>();
        private readonly IBuyInComingApplyBll _buyInComingApplyBll = UnityContainerHelper.Server<IBuyInComingApplyBll>();
        private readonly IBaseCompanyBll _baseCompanyBll = UnityContainerHelper.Server<IBaseCompanyBll>();
        private readonly IUseApplyBll _useApplyBll = UnityContainerHelper.Server<IUseApplyBll>();
        private readonly IBaseApplyBll _baseApplyBll = UnityContainerHelper.Server<IBaseApplyBll>();
        private readonly ITypeBll _typeBll = UnityContainerHelper.Server<ITypeBll>();
        private readonly ICompanyBll _companyBll = UnityContainerHelper.Server<ICompanyBll>();
        private readonly IBaseInfoBll _baseInfoBll = UnityContainerHelper.Server<IBaseInfoBll>();

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
                BuyInComing_Apply buyInComingApply = _buyInComingApplyBll.Find( applyInfo.Apply_Id);
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
                Material_Apply materialApply = _baseApplyBll.Find( applyInfo.Apply_Id);
                materialApply.Is_Get = true;
                materialApplies.Add(materialApply);
            }

            return _baseApplyBll.UpdateEntities(materialApplies) ? "OK" : "Error";
        }

        #endregion 可出库

        #region 申请入库

        public ActionResult SelfApplyInComing()
        {
            ViewBag.types = _typeBll.GetEntities(p => p.Material_Type_Parent_Id == "000000");
            ViewBag.companys = _companyBll.GetEntities(p => true).Select(p => p.Company_Name);
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public string SelfApplyInComing(Material_Info materialInfo, int applyCount)
        {
            if (_baseInfoBll.GetEntities(m => m.Material_Id == materialInfo.Material_Id) == null)
            {
                materialInfo.Material_Count = 0;
                materialInfo.Material_RemainCont = 0;
                materialInfo.Is_Show = false;
                if (_baseInfoBll.AddBaseInfo(materialInfo) != "添加成功")
                {
                    return "Error";
                }
            }
            BuyInComing_Apply buyInComingApply = new BuyInComing_Apply()
            {
                Material_Id = materialInfo.Material_Id,
                Is_InComed = false,
                Is_Bought = false,
                Last_Time = DateTime.MaxValue
            };

            if (_buyInComingApplyBll.AddEntities(new List<BuyInComing_Apply>() { buyInComingApply }))
            {
                Apply_Info applyInfo = new Apply_Info
                {
                    Apply_Id = buyInComingApply.Id,
                    ApplyType_Id = "002",
                    Apply_Count = applyCount,
                    Apply_Status = 0
                };
                if (_applyInfoBll.AddEntities(new List<Apply_Info> { applyInfo }))
                {
                    return "OK";
                }
            }

            return "Error";
        }

        [HttpPost]
        public string GetChildType(string typeId)
        {
            List<Material_Type> types = _typeBll.GetEntities(m => m.Material_Type_Parent_Id == typeId);
            if (types != null)
            {
                return JsonConvert.SerializeObject(types);
            }
            return "";
        }

        [HttpPost]
        public string GetMaterialsByType(string typeId)
        {
            string name = _typeBll.Find(typeId).Material_Type_Name;
            List<Material_Info> materialInfos = _baseInfoBll.GetEntities(m => m.Material_Type_Name == name);

            return JsonConvert.SerializeObject(materialInfos);
        }

        [HttpPost]
        public string GetNewMaterialId()
        {
            Random random = new Random();
            string newId = random.Next(0, 333333).ToString();
            bool isContain = true;
            while (isContain)
            {
                if (_baseInfoBll.GetEntity(m => m.Material_Id == newId) == null)
                {
                    isContain = false;
                }
                newId = random.Next(0, 333333).ToString();
            }

            return newId;
        }

        #endregion 申请入库
    }
}