using ZERO.Material.Command;
using ZERO.Material.IDal;

namespace ZERO.Material.Dal.Factory
{
    public class StaticDalFactory
    {
        private static readonly UnityContainerHelper Container = new UnityContainerHelper();

        public static IBaseDal GetIBaseDal() => Container.Server<IBaseDal>();

        public static IBaseCompanyDal GetIBaseCompanyDal() => Container.Server<IBaseCompanyDal>();

        public static IBaseInfoDal GetBaseInfoDal() => Container.Server<IBaseInfoDal>();

        public static IBaseTypeDal GetBaseTypeDal() => Container.Server<IBaseTypeDal>();

        public static ICompanyDal GetCompanyDal() => Container.Server<ICompanyDal>();

        public static ITypeDal GeTypeDal() => Container.Server<ITypeDal>();

        public static IBaseApplyDal GetBaseApplyDal() => Container.Server<IBaseApplyDal>();

        public static IApplyTypeDal GetApplyTypeDal() => Container.Server<IApplyTypeDal>();

        public static IApplyInfoDal GetApplyInfoDal() => Container.Server<IApplyInfoDal>();

        public static IBuyInComingApplyDal GetBuyInComingApplyDal() => Container.Server<IBuyInComingApplyDal>();

        public static ITeacherDal GetTeacherDal() => Container.Server<ITeacherDal>();

        public static IUseApplyDal GetUseApplyDal() => Container.Server<IUseApplyDal>();

        public static IBuyApplyDal GetBuyApplyDal() => Container.Server<IBuyApplyDal>();

        public static IActionDal GetActionDal() => Container.Server<IActionDal>();

        public static IRoleDal GetRoleDal() => Container.Server<IRoleDal>();

        public static IRoleActionDal GetRoleActionDal() => Container.Server<IRoleActionDal>();

        public static IRoleTeacherDal GetRoleTeacherDal() => Container.Server<IRoleTeacherDal>();

        public static ITeacherActionDal GetTeacherActionDal() => Container.Server<ITeacherActionDal>();
    }
}