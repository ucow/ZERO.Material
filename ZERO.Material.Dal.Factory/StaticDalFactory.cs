using ZERO.Material.Command;
using ZERO.Material.IDal;

namespace ZERO.Material.Dal.Factory
{
    public class StaticDalFactory
    {
        public static IBaseDal GetIBaseDal() => UnityContainerHelper.Server<IBaseDal>();

        public static IBaseCompanyDal GetIBaseCompanyDal() => UnityContainerHelper.Server<IBaseCompanyDal>();

        public static IBaseInfoDal GetBaseInfoDal() => UnityContainerHelper.Server<IBaseInfoDal>();

        public static IBaseTypeDal GetBaseTypeDal() => UnityContainerHelper.Server<IBaseTypeDal>();

        public static ICompanyDal GetCompanyDal() => UnityContainerHelper.Server<ICompanyDal>();

        public static ITypeDal GeTypeDal() => UnityContainerHelper.Server<ITypeDal>();

        public static IBaseApplyDal GetBaseApplyDal() => UnityContainerHelper.Server<IBaseApplyDal>();

        public static IApplyTypeDal GetApplyTypeDal() => UnityContainerHelper.Server<IApplyTypeDal>();

        public static IApplyInfoDal GetApplyInfoDal() => UnityContainerHelper.Server<IApplyInfoDal>();

        public static IBuyInComingApplyDal GetBuyInComingApplyDal() => UnityContainerHelper.Server<IBuyInComingApplyDal>();

        public static ITeacherDal GetTeacherDal() => UnityContainerHelper.Server<ITeacherDal>();

        public static IUseApplyDal GetUseApplyDal() => UnityContainerHelper.Server<IUseApplyDal>();

        public static IBuyApplyDal GetBuyApplyDal() => UnityContainerHelper.Server<IBuyApplyDal>();

        public static IActionDal GetActionDal() => UnityContainerHelper.Server<IActionDal>();

        public static IRoleDal GetRoleDal() => UnityContainerHelper.Server<IRoleDal>();

        public static IRoleActionDal GetRoleActionDal() => UnityContainerHelper.Server<IRoleActionDal>();

        public static IRoleTeacherDal GetRoleTeacherDal() => UnityContainerHelper.Server<IRoleTeacherDal>();

        public static ITeacherActionDal GetTeacherActionDal() => UnityContainerHelper.Server<ITeacherActionDal>();
    }
}