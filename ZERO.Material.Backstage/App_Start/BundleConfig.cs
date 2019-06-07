using System.Web.Optimization;

namespace ZERO.Material.Backstage
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            #region Jquery

            //jquery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //jquery验证
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            #endregion Jquery

            #region Bootstrap

            //bootstrap脚本
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/toastr.min.js"));

            //bootstrap脚本样式
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/toastr.min.css"));

            #endregion Bootstrap

            #region Layui

            //layui脚本
            bundles.Add(new ScriptBundle("~/bundles/layui").Include("~/Content/layui/layui.js"));
            //layui样式
            bundles.Add(new StyleBundle("~/Content/layui").Include("~/Content/layui/css/layui.css"));

            #endregion Layui

            #region Login/Regist

            bundles.Add(new Bundle("~/assets/css").Include(
                "~/assets/css/bootstrap.min.css",
                "~/assets/font-awesome/4.2.0/css/font-awesome.min.css",
                "~/assets/fonts/fonts.googleapis.com.css",
                "~/assets/css/ace.min.css",
                "~/assets/css/ace-rtl.min.css",
                "~/Content/toastr.min.css"));

            bundles.Add(new ScriptBundle("~/assets/js").Include(
                "~/assets/js/jquery.2.1.1.min.js",
                "~/assets/js/jquery.min.js",
                "~/assets/js/jquery.mobile.custom.min.js",
                "~/Scripts/toastr.min.js"));

            #endregion Login/Regist
        }
    }
}