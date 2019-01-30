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

        private static readonly UnityContainerHelper Container = new UnityContainerHelper();

        private readonly IBaseApplyBll _applyBll = Container.Server<IBaseApplyBll>();
        private readonly ITeacherBll _teacherBll = Container.Server<ITeacherBll>();
        private readonly IApplyInfoBll _applyInfoBll = Container.Server<IApplyInfoBll>();
        private readonly IBaseInfoBll _infoBll = Container.Server<IBaseInfoBll>();
        private readonly IUseApplyBll _useApplyBll = Container.Server<IUseApplyBll>();
        private readonly IBuyApplyBll _buyApplyBll = Container.Server<IBuyApplyBll>();

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
            IUseApplyBll useApplyBll = Container.Server<IUseApplyBll>();
            List<Use_Apply> useApplies = useApplyBll.GetEntities(m =>
                    m.Is_Get == 0 &&
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

        public string GetMaterialBuy(int page, int limit, string status)
        {
            int? state = status == null ? (int?)null : Int32.Parse(status);
            List<Buy_Apply> buyApplies = _buyApplyBll.GetEntities(m => state == null || m.Apply_Status == state).Skip((page - 1) * limit).Take(limit).ToList();

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
            if (_applyInfoBll.UpdateEntities(applies))
            {
                return Content("OK");
            }
            return Content("Error");
        }
    }
}