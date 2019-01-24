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

        private readonly IBaseInfoBll _infoBll = Container.Server<IBaseInfoBll>();

        #endregion 全局变量

        #region 申请列表

        // GET: Apply
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
            List<Material_Apply> materApplies = _applyBll.GetEntities(m =>
                (applyFilter.Teacher == null || m.Teacher_Id == applyFilter.Teacher) &&
                (status == null || m.Head_Aduit == status) &&
                ((applyTime0 == null || applyTime1 == null) || m.Apply_Time >= applyTime0 && m.Apply_Time <= applyTime1) &&
                ((startTime0 == null || startTime1 == null) || m.Start_Time >= startTime0 && m.Start_Time <= startTime1) &&
                ((endTime0 == null || endTime1 == null) || m.End_Time >= endTime0 && m.End_Time <= endTime1))
                .Skip((page - 1) * limit).Take(limit).ToList();

            foreach (Material_Apply materApply in materApplies)
            {
                Material_Teacher teacher = _teacherBll.GetEntity(m => m.Teacher_Id == materApply.Teacher_Id);
                materApply.Teacher_Name = teacher.Teacher_Name;
                materApply.Teacher_Depart = teacher.Teacher_Depart;
                materApply.Material_Name =
                    _infoBll.GetEntity(m => m.Material_Id == materApply.Material_Id).Material_Name;
            }

            var msg = new
            {
                code = 0,
                msg = "",
                total = materApplies.Count,
                data = materApplies
            };
            return JsonConvert.SerializeObject(msg);
        }

        #endregion 申请列表

        public ActionResult Review(List<Material_Apply> applies)
        {
            if (_applyBll.UpdateEntities(applies))
            {
                return Content("OK");
            }
            return Content("Error");
        }
    }
}