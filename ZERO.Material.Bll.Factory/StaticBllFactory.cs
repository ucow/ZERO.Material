using ZERO.Material.Command;
using ZERO.Material.IBll;

namespace ZERO.Material.Bll.Factory
{
    public class StaticBllFactory
    {
        private static readonly UnityContainerHelper Container = new UnityContainerHelper();

        public static IBaseBll GetIBaseBll() => Container.Server<IBaseBll>();

        public static IBaseCompanyBll GetIBaseCompanyBll() => Container.Server<IBaseCompanyBll>();

        public static IBaseInfoBll GetBaseInfoBll() => Container.Server<IBaseInfoBll>();

        public static IBaseTypeBll GetBaseTypeBll() => Container.Server<IBaseTypeBll>();

        public static ICompanyBll GetCompanyBll() => Container.Server<ICompanyBll>();

        public static ITypeBll GeTypeBll() => Container.Server<ITypeBll>();

        public static IBaseApplyBll GetBaseApplyBll() => Container.Server<IBaseApplyBll>();

        public static IApplyTypeBll GetApplyTypeBll() => Container.Server<IApplyTypeBll>();

        public static IApplyInfoBll GetApplyInfoBll() => Container.Server<IApplyInfoBll>();

        public static IBuyInComingApplyBll GetBuyInComingApplyBll() => Container.Server<IBuyInComingApplyBll>();

        public static ITeacherBll GetTeacherBll() => Container.Server<ITeacherBll>();

        public static IUseApplyBll GetUseApplyBll() => Container.Server<IUseApplyBll>();

        public static IBuyApplyBll GetBuyApplyBll() => Container.Server<IBuyApplyBll>();

        public static IActionBll GetActionBll() => Container.Server<IActionBll>();

        public static IRoleBll GetRoleBll() => Container.Server<IRoleBll>();

        public static IRoleActionBll GetRoleActionBll() => Container.Server<IRoleActionBll>();

        public static IRoleTeacherBll GetRoleTeacherBll() => Container.Server<IRoleTeacherBll>();

        public static ITeacherActionBll GetTeacherActionBll() => Container.Server<ITeacherActionBll>();
    }
}