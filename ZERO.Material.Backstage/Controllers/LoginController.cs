using System.Collections.Generic;
using System.Web.Mvc;
using ZERO.Material.Backstage.Filter;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.Model;

namespace ZERO.Material.Backstage.Controllers
{
    [CheckLogin(IsChecked = false)]
    public class LoginController : Controller
    {
        #region 全局变量

        private readonly ITeacherBll _teacherBll = UnityContainerHelper.Server<ITeacherBll>();
        private readonly IRoleTeacherBll _roleTeacherBll = UnityContainerHelper.Server<IRoleTeacherBll>();
        private readonly IRoleBll _roleBll = UnityContainerHelper.Server<IRoleBll>();

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
            Material_Teacher teacher = _teacherBll.GetEntity(m => (m.Teacher_Id == username || m.Teacher_Name == username) && m.Del_Flag == false);
            if (teacher == null || teacher.Teacher_Password != password)
            {
                return Content("用户名或密码错误，请核对");
            }
            Material_Role_Teacher roleTeacher = _roleTeacherBll.GetEntity(m => m.Teacher_Id == teacher.Teacher_Id);
            if (roleTeacher != null)
            {
                int roleId = roleTeacher.Role_Id;
                bool delFlag = _roleBll.Find(roleId).Del_Flag;
                if (teacher == null || delFlag)
                {
                    return Content("该用户不存在或已被冻结，请先注册");
                }
            }

            return Content(teacher.Teacher_Password != password ? "密码错误，请核对密码" : "OK");
        }

        [HttpPost]
        public ActionResult BeforeRegister(Material_Teacher teacher)
        {
            return Content(_teacherBll.AddEntities(new List<Material_Teacher>() { teacher }) ? "OK" : "Error");
        }

        #endregion 前台登录

        #region 后台登录

        public ActionResult BackstageLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BackstageLogin(string username, string password, string validateCode)
        {
            if (validateCode != Session["ValidateCode"].ToString())
            {
                return Content("验证码错误");
            }

            Material_Teacher teacher = _teacherBll.GetEntity(m =>
                (m.Teacher_Name == username || m.Teacher_Id == username) && m.Teacher_Password == password && !m.Del_Flag);
            if (teacher == null || teacher.Teacher_Password != password)
            {
                return Content("用户名或密码错误，请核对");
            }
            Material_Role_Teacher roleTeacher = _roleTeacherBll.GetEntity(m => m.Teacher_Id == teacher.Teacher_Id);
            if (roleTeacher != null)
            {
                int roleId = roleTeacher.Role_Id;
                bool delFlag = _roleBll.Find(roleId).Del_Flag;
                if (teacher == null || delFlag)
                {
                    return Content("该用户不存在或已被冻结，请先注册");
                }
            }

            return Content("OK");
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