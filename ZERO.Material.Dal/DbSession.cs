using ZERO.Material.Dal.Factory;
using ZERO.Material.IDal;

namespace ZERO.Material.Dal
{
    public class DbSession : IDbSession
    {
        public IBaseDal BaseDal()
        {
            return StaticDalFactory.GetIBaseDal();
        }

        public IBaseCompanyDal BaseCompanyDal()
        {
            return StaticDalFactory.GetIBaseCompanyDal();
        }

        public IBaseInfoDal BaseInfoDal()
        {
            return StaticDalFactory.GetBaseInfoDal();
        }

        public IBaseTypeDal BaseTypeDal()
        {
            return StaticDalFactory.GetBaseTypeDal();
        }

        public ICompanyDal CompanyDal()
        {
            return StaticDalFactory.GetCompanyDal();
        }

        public ITypeDal TypeDal()
        {
            return StaticDalFactory.GeTypeDal();
        }

        public IBaseApplyDal BaseApplyDal()
        {
            return StaticDalFactory.GetBaseApplyDal();
        }

        public ITeacherDal TeacherDal() => StaticDalFactory.GetTeacherDal();

        public IApplyInfoDal ApplyInfoDal()
        {
            return StaticDalFactory.GetApplyInfoDal();
        }

        public IApplyTypeDal ApplyTypeDal()
        {
            return StaticDalFactory.GetApplyTypeDal();
        }

        public IBuyInComingApplyDal BuyInComingApplyDal()
        {
            return StaticDalFactory.GetBuyInComingApplyDal();
        }

        public IUseApplyDal UseApplyDal()
        {
            return StaticDalFactory.GetUseApplyDal();
        }

        public IBuyApplyDal BuyApplyDal()
        {
            return StaticDalFactory.GetBuyApplyDal();
        }

        public int SaveChanges()
        {
            return DbContextFactory.GetDbContext().SaveChanges();
        }
    }
}