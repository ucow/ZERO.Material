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

        public int SaveChanges()
        {
            return DbContextFactory.GetDbContext().SaveChanges();
        }
    }
}