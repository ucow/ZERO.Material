using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;
using ZERO.Material.Model.Other;

namespace ZERO.Material.Backstage.Controllers
{
    public class ApplyController : Controller
    {
        #region 全局变量

        private readonly IBaseApplyBll _applyBll = UnityContainerHelper.Server<IBaseApplyBll>();
        private readonly ITeacherBll _teacherBll = UnityContainerHelper.Server<ITeacherBll>();
        private readonly IApplyInfoBll _applyInfoBll = UnityContainerHelper.Server<IApplyInfoBll>();
        private readonly IBuyApplyBll _buyApplyBll = UnityContainerHelper.Server<IBuyApplyBll>();
        private readonly IBaseCompanyBll _baseCompanyBll = UnityContainerHelper.Server<IBaseCompanyBll>();

        #endregion 全局变量

        #region 借用申请列表

        // GET: Apply
        //借用审批
        public ActionResult Borrow()
        {
            List<string> teacherIds = _applyBll.GetEntities(m => true).Select(m => m.Teacher_Id).Distinct().ToList();
            for (int i = 0; i < teacherIds.Count; i++)
            {
                var id = teacherIds[i];
                teacherIds[i] += "/" + _teacherBll.GetEntity(m => m.Teacher_Id == id).Teacher_Name;
            }

            ViewBag.teachers = teacherIds;
            return View();
        }

        //获取借用信息
        public string GetMaterialBorrow(int page, int limit, ApplyFilter applyFilter)
        {
            DateTime[] applyTimes = applyFilter.GetStartAndEndTime(applyFilter.ApplyTime);
            DateTime? applyTime0 = applyTimes?[0];
            DateTime? applyTime1 = applyTimes?[1];
            DateTime[] startTimes = applyFilter.GetStartAndEndTime(applyFilter.StartTime);
            DateTime? startTime0 = startTimes?[0];
            DateTime? startTime1 = startTimes?[1];
            DateTime[] endTimes = applyFilter.GetStartAndEndTime(applyFilter.EndTime);
            DateTime? endTime0 = endTimes?[0];
            DateTime? endTime1 = endTimes?[1];
            int? status = applyFilter.Status == null ? (int?)null : Int32.Parse(applyFilter.Status);
            string teacher = applyFilter.Teacher == null
                ? null
                : _teacherBll.GetEntity(m => m.Teacher_Id == applyFilter.Teacher).Teacher_Name;
            IUseApplyBll useApplyBll = UnityContainerHelper.Server<IUseApplyBll>();
            List<Use_Apply> useApplies = useApplyBll.GetEntities(m =>
                    m.Is_Get == false &&
                    (applyFilter.Teacher == null || m.Teacher_Name == teacher) &&
                    (status == null || m.Apply_Status == status) &&
                    ((applyTime0 == null || applyTime1 == null) || m.Apply_Time >= applyTime0 && m.Apply_Time <= applyTime1) &&
                    ((startTime0 == null || startTime1 == null) || m.Start_Time >= startTime0 && m.Start_Time <= startTime1) &&
                    ((endTime0 == null || endTime1 == null) || m.End_Time >= endTime0 && m.End_Time <= endTime1))
                .Skip((page - 1) * limit).Take(limit).ToList();

            var msg = new
            {
                code = 0,
                msg = "",
                total = useApplies.Count,
                data = useApplies
            };
            return JsonConvert.SerializeObject(msg);
        }

        #endregion 借用申请列表

        #region 采购申请

        public ActionResult Buy()
        {
            return View();
        }

        public string GetMaterialBuyInComing(int page, int limit, string status, string applyType)
        {
            int? state = status == null ? (int?)null : Int32.Parse(status);
            bool isBought = applyType == "003";
            List<Buy_Apply> buyApplies = _buyApplyBll.GetEntities(m => m.Is_InComed == false && m.Is_Bought == isBought && m.ApplyType_Id == applyType && (state == null || m.Apply_Status == state)).Skip((page - 1) * limit).Take(limit).ToList();

            var msg = new
            {
                code = 0,
                msg = "",
                total = buyApplies.Count,
                data = buyApplies
            };
            return JsonConvert.SerializeObject(msg);
        }

        #endregion 采购申请

        public ActionResult InComing()
        {
            return View();
        }

        //审核
        public ActionResult Review(List<Apply_Info> applies)
        {
            List<Material_Base_Company> materialBaseCompanies = new List<Material_Base_Company>();
            foreach (Apply_Info applyInfo in applies)
            {
                if (applyInfo.ApplyType_Id == "001" && applyInfo.Apply_Status == 2)
                {
                    Material_Apply apply = _applyBll.GetEntity(m => m.Id == applyInfo.Apply_Id);
                    Material_Base_Company materialBaseCompany =
                        _baseCompanyBll.GetEntity(m => m.Material_Id == apply.Material_Id);
                    materialBaseCompany.Material_RemainCont += applyInfo.Apply_Count;
                    materialBaseCompanies.Add(materialBaseCompany);
                }
            }
            return Content(_applyInfoBll.UpdateEntities(applies) && _baseCompanyBll.UpdateEntities(materialBaseCompanies) ? "OK" : "Error");
        }
    }
}