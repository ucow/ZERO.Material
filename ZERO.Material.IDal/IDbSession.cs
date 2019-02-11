using ZERO.Material.Model;

namespace ZERO.Material.IDal
{
    public interface IDbSession
    {
        IBaseDal BaseDal();

        IBaseCompanyDal BaseCompanyDal();

        IBaseInfoDal BaseInfoDal();

        IBaseTypeDal BaseTypeDal();

        ICompanyDal CompanyDal();

        ITypeDal TypeDal();

        IBaseApplyDal BaseApplyDal();

        ITeacherDal TeacherDal();

        IApplyInfoDal ApplyInfoDal();

        IApplyTypeDal ApplyTypeDal();

        IBuyInComingApplyDal BuyInComingApplyDal();

        IUseApplyDal UseApplyDal();

        IBuyApplyDal BuyApplyDal();

        IActionDal ActionDal();

        IRoleDal RoleDal();

        IRoleActionDal RoleActionDal();

        IRoleTeacherDal RoleTeacherDal();

        ITeacherActionDal TeacherActionDal();

        int SaveChanges();
    }
}