﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    public class LoginController : Controller
    {
        #region 全局变量

        private static readonly UnityContainerHelper Container = new UnityContainerHelper();

        private ITeacherBll _teacherBll = Container.Server<ITeacherBll>();

        #endregion 全局变量

        // GET: Login

        #region 前台登录

        public ActionResult BeforeLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BeforeLogin(string username, string password)
        {
            Material_Teacher teacher = _teacherBll.GetEntity(m => m.Teacher_Id == username);
            if (teacher == null)
            {
                return Content("该用户不存在，请先注册");
            }
            else if (teacher.Teacher_Password != password)
            {
                return Content("密码错误，请核对密码");
            }

            return Content("OK");
        }

        [HttpPost]
        public ActionResult BeforeRegister(Material_Teacher teacher)
        {
            if (_teacherBll.AddEntities(new List<Material_Teacher>() { teacher }))
            {
                return Content("OK");
            }

            return Content("Error");
        }

        #endregion 前台登录

        #region 后台登录

        public ActionResult BackstageLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BackstageLogin()
        {
            return View();
        }

        #endregion 后台登录

        //获取验证码
        public ActionResult GetValidateCode()
        {
            ValidateCode vCode = new ValidateCode();
            string code = vCode.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
    }
}