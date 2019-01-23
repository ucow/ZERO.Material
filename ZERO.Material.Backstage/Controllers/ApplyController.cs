using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

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
            return View();
        }

        public string GetMaterialBorrow(int page, int limit)
        {
            List<Material_Apply> materApplies = _applyBll.GetEntities(m => true).Skip((page - 1) * limit).Take(limit).ToList();
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

        public ActionResult Review(Material_Apply apply)
        {
            if (_applyBll.UpdateEntities(new List<Material_Apply>() { apply }))
            {
                return Content("OK");
            }
            return Content("Error");
        }
    }
}